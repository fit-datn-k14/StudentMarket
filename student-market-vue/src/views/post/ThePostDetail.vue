<template>
  <div class="single-product mt-150 mb-150">
    <div class="container tpd__post">
      <div class="row">
        <div class="col-md-5">
          <div class="single-product-img">
            <img
              class="ppd__img border border-dark"
              :src="URL + `Images/posts/${id}/${ImageID}`"
            />
            <div class="imglist">
              <img
                v-for="(imgID, indexImg) in Post.ListImages"
                :key="indexImg"
                :src="URL + `Images/posts/${id}/${imgID}`"
                @click="ImageID = imgID"
              />
            </div>
          </div>
        </div>
        <div class="col-md-7">
          <div class="single-product-content">
            <div class="tpd__title">
              <h3>{{ Post.Title }}</h3>
              <router-link :to="`/chinh-sua-tin-dang/${id}`">
                <h-button
                  v-if="Post.UserID == User.UserID"
                  class="edit-post"
                  type="btn-pri"
                  value="Chỉnh Sửa"
                />
              </router-link>
            </div>
            <p v-if="Post.Price" class="single-product-pricing">
              {{ Post.Price }} đ
            </p>
            <p v-else class="single-product-pricing">Liên hệ</p>
            <p class="single-product-categories">
              <strong>Danh Mục: </strong>{{ Post.CategoryName }}
            </p>
            <p v-if="Post.Address" class="single-product-categories">
              <strong>Địa Chỉ: </strong>{{ Post.Address }}
            </p>
            <strong>Chi Tiết Sản Phẩm:</strong>
            <p v-if="Post.PostDescribe" class="single-product-describe">
              {{ Post.PostDescribe }}
            </p>
            <p v-else class="single-product-describe">Không có mô tả</p>
          </div>
        </div>
      </div>
    </div>
    <div class="container tpd__seller">
      <div>
        <img
          class="seller__avatar border border-dark"
          :src="URL + `Images/users/avatar/${this.Seller.Avatar}`"
        />
        <div>
          <div class="seller__name">
            {{ Seller.FullName }}
          </div>
          <div class="btn-mess" v-if="Post.UserID != User.UserID">
            <router-link :to="`/tin-nhan/${Post.UserID}`">Nhắn Tin</router-link>
          </div>
        </div>
      </div>
      <div>
        <p class="">
          <strong>Số Điện Thoại: </strong>
          <span v-if="Seller.PhoneNumber">{{ Seller.PhoneNumber }}</span>
          <span v-else>Chưa có</span>
        </p>
        <p class="">
          <strong>Email: </strong>
          <span v-if="Seller.Email">{{ Seller.Email }}</span>
          <span v-else>Chưa có</span>
        </p>
        <p class="">
          <strong>Địa Chỉ: </strong>
          <span v-if="Seller.LocationName">{{ Seller.LocationName }}</span>
          <span v-else>Chưa có</span>
        </p>
      </div>
    </div>
    <div class="container tpd__comments">
      <div>
        <h2>Bình luận</h2>
        <div class="listcomments">
          <div v-for="comment in Comments" :key="comment.CommentID">
            <img
              class="user__avatar border border-dark"
              :src="URL + `Images/users/avatar/${comment.Avatar}`"
            />
            <div>
              <div class="cmt__fullname">
                {{ comment.FullName }}
                <span class="comment__time"
                  >&nbsp;{{
                    HCommon.formatTime(comment.CreatedDate)
                  }}&nbsp;</span
                >
              </div>
              <div class="cmt__content">{{ comment.Content }}</div>
            </div>
          </div>
        </div>
        <div class="addCommentBox">
          <img
            v-if="User"
            class="user__avatar border border-dark"
            :src="URL + `Images/users/avatar/${User.Avatar}`"
          />
          <img
            v-else
            class="user__avatar border border-dark"
            :src="URL + `Images/users/avatar/null`"
          />
          <h-input
            ref="txtNewComment"
            type="textarea"
            v-model="newComment.Content"
            @keyup.enter="addComment"
            placeholder="Viết bình luận..."
          />
          <div class="icon-send" @click="addComment">
            <i class="fa-regular fa-paper-plane"></i>
          </div>
        </div>
      </div>
    </div>
  </div>
  <h-loading v-if="isLoading"></h-loading>
</template>
  <script>
import HConfig from "@/js/base/config";
import HInput from "@/components/input/HInput.vue";
import { getUserFromLocalStorage } from "@/stores/localStorage.js";
export default {
  components: { HInput },
  name: "ThePostDetail",
  props: {
    id: {
      type: String,
      required: true,
    },
  },
  async created() {
    this.User = getUserFromLocalStorage();
    await this.loadData();
    this.ImageID = this.Post.ImageName;
    await this.getSeller();
    await this.getComments();
    this.newComment.PostID = this.id;
    this.newComment.UserID = this.User.UserID;
    this.newComment.Content = null;
  },
  methods: {
    addComment() {
      if (this.newComment.Content) {
        var url = HConfig.API.Comments;
        this.axios
          .post(url, this.newComment)
          .then((response) => {
            if (response.data.Success) {
              this.newComment.Content = null;
              this.getComments();
            }
          })
          .catch((error) => {
            this.errorMessage = error;
          });
      }
    },

    async loadData() {
      this.isLoading = true;
      try {
        var url = this.HConfig.API.Posts + this.id;
        await this.axios.get(url).then((response) => {
          if (response.data.Success) {
            this.Post = response.data.Data;
            this.isLoading = false;
          } else {
            this.errorMessage = response.data.UserMsg;
            this.dialogType = this.HEnum.DialogType.Error;
          }
        });
      } catch (error) {
        this.errorMessage = this.HResource.Text.MessageException;
        this.dialogType = this.HEnum.DialogType.Error;
      }
    },
    async getSeller() {
      this.isLoading = true;
      try {
        var url = this.HConfig.API.Users + this.Post.UserID;
        await this.axios.get(url).then((response) => {
          if (response.data.Success) {
            this.Seller = response.data.Data;
            this.isLoading = false;
          } else {
            this.errorMessage = response.data.UserMsg;
            this.dialogType = this.HEnum.DialogType.Error;
          }
        });
      } catch (error) {
        this.errorMessage = this.HResource.Text.MessageException;
        this.dialogType = this.HEnum.DialogType.Error;
      }
    },
    async getComments() {
      this.isLoading = true;
      try {
        var url = this.HConfig.API.Comments + this.Post.PostID;
        await this.axios.get(url).then((response) => {
          if (response.data.Success) {
            this.Comments = response.data.Data;
            this.isLoading = false;
          } else {
            this.errorMessage = response.data.UserMsg;
            this.dialogType = this.HEnum.DialogType.Error;
          }
        });
      } catch (error) {
        this.errorMessage = this.HResource.Text.MessageException;
        this.dialogType = this.HEnum.DialogType.Error;
      }
    },
  },

  data() {
    return {
      URL: HConfig.URL,
      User: {},
      Post: {},
      Seller: {},
      Comments: [],
      newComment: {},
      ImageID: null,
      isLoading: true,
    };
  },
};
</script>

<style scoped>
.addCommentBox {
  display: flex;
  padding-top: 12px;
}

.tpd__post {
  position: relative;
}
.listcomments {
  display: flex;
  flex-direction: column;
  flex-direction: column-reverse;
  row-gap: 8px;
}
.listcomments > div {
  display: flex;
  padding: 12px 0;
  align-items: start;
  border-bottom: 1px solid var(--border-color);
}

.cmt__fullname {
  font-weight: bold;
}

.comment__time {
  font-weight: normal;
}

.tpd__comments img {
  width: 40px;
  height: 40px;
  border-radius: 20px;
  margin-right: 12px;
  aspect-ratio: 1 / 1;
  -o-object-fit: cover;
  object-fit: cover;
}
.addCommentBox {
  position: relative;
}
.addCommentBox > .icon-send {
  position: absolute;
  right: 8px;
  bottom: 0;
  width: 36px;
  height: 36px;
  display: flex;
  justify-content: center;
  align-items: center;
  cursor: pointer;
}
.addCommentBox i {
  font-size: 24px;
  text-align: center;
  color: var(--primary-color);
}
.tpd__seller {
  height: 132px;
  display: flex;
  column-gap: 16px;
}
.tpd__seller > div:first-child {
  display: flex;
  align-items: center;
  column-gap: 8px;
  width: 40%;
}
.tpd__seller > div:last-child {
  display: flex;
  align-items: center;
  column-gap: 20px;
  width: 40%;
}

.tpd__seller span {
  display: block;
}

.seller__name {
  font-size: 24px;
  font-weight: 600;
}
.seller__avatar {
  height: 84px;
  width: 84px;
  border-radius: 42px;
  aspect-ratio: 1 / 1;
  -o-object-fit: cover;
  object-fit: cover;
}
.ppd__img {
  width: 100%;
}

.single-product-describe {
  min-height: 180px;
}
.imglist {
  display: flex;
  column-gap: 2%;
  margin-top: 16px;
}
.imglist img {
  width: 15%;
  box-sizing: border-box;
  border: 1px solid var(--border-color);
}

.single-product {
  padding: 70px 0;
}

.single-product > div {
  background-color: #fff;
  padding: 24px;
  border-radius: 4px;
  margin-bottom: 12px;
}

.single-product-img {
  width: 100%;
}

.single-product-content {
  padding-left: 40px;
}

.single-product-content h3 {
  font-size: 36px;
  font-weight: 700;
  margin-bottom: 20px;
}

.single-product-pricing {
  font-size: 24px;
  font-weight: 600;
  margin-bottom: 30px;
  color: var(--red-color);
}

.single-product-pricing span {
  font-size: 18px;
  margin-right: 10px;
}

.single-product-content p {
  font-size: 18px;
  line-height: 28px;
  margin-bottom: 30px;
}

.btn-mess {
  background-color: var(--primary-color);
  width: 96px;
  height: 32px;
  display: flex;
  justify-content: center;
  align-items: center;
  border-radius: 16px;
}

.btn-mess a {
  color: #fff;
}

.cart-btn {
  display: inline-block;
  padding: 10px 30px;
  font-size: 18px;
  font-weight: 700;
  background-color: #000;
  color: #fff;
  border-radius: 30px;
  transition: all 0.3s ease;
}

.cart-btn:hover {
  transform: translateY(-3px);
  box-shadow: 0 5px 15px rgba(0, 0, 0, 0.3);
}

.single-product-content strong {
  font-weight: 700;
  font-size: 18px;
}

.single-product-categories {
}

.product-share {
  display: flex;
  align-items: center;
  margin-top: 40px;
}

body {
  font-family: "Open Sans", sans-serif;
  font-weight: 400;
  font-size: 1rem;
  letter-spacing: 0.1px;
  line-height: 1.8;
  color: #051922;
  overflow-x: hidden;
}

.edit-post {
  top: -24px;
  right: 24px;
}
.tpd__title {
  display: flex;
  justify-content: space-between;
}
</style>