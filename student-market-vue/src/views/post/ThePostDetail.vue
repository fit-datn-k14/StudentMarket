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
            </div>
            <p v-if="Post.Price" class="single-product-pricing">
              {{ HCommon.formatMoney(Post.Price) }}
            </p>
            <p v-else class="single-product-pricing">Liên hệ</p>
            <p class="single-product-categories">
              <strong>Danh Mục: </strong>{{ Post.CategoryName }}
            </p>
            <p class="single-product-categories">
              <strong>Khu vực: </strong>{{ Post.LocationName }}
            </p>
            <p v-if="Post.Address" class="single-product-categories">
              <strong>Địa Chỉ: </strong>{{ Post.Address }}
            </p>
            <strong>Chi Tiết Sản Phẩm:</strong>
            <p v-if="Post.PostDescribe" class="single-product-describe">
              {{ Post.PostDescribe }}
            </p>
            <p v-else class="single-product-describe">Không có mô tả</p>
            <div class="post-actions">
              <div class="post__heart">
                <i
                  v-if="listIdFavouritePosts.includes(Post.PostID)"
                  class="fa-solid fa-heart"
                  @click.prevent="onClickUnFavouritePosts($event)"
                ></i>
                <i
                  v-else
                  class="fa-regular fa-heart"
                  @click.prevent="onClickFavouritePosts($event)"
                ></i>
                Đã thích ({{ Post.NumberFavourite }})
              </div>
              <router-link :to="`/chinh-sua-tin-dang/${id}`">
                <h-button
                  v-if="User && Post.UserID == User.UserID"
                  class="edit-post"
                  type="btn-pri"
                  value="Chỉnh Sửa"
                />
              </router-link>
              <h-button
                v-if="User && Post.UserID == User.UserID"
                class="delete-post"
                type="btn-pri"
                value="Xoá"
                @click="onClickDelete"
              />
            </div>
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
          <div class="btn-mess" v-if="User && Post.UserID != User.UserID">
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
        <div v-if="Comments.length > 0" class="listcomments">
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
        <div v-else class="listcomments">Chưa có bình luận nào</div>
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
            :disabled="!User"
          />
          <div v-if="User" class="icon-send" @click="addComment">
            <i class="fa-regular fa-paper-plane"></i>
          </div>
        </div>
      </div>
    </div>
  </div>
  <h-dialog
    v-if="errorMessage"
    :content="errorMessage"
    :type="dialogType"
    @dialogEvent="dialogEvent"
  />
  <h-loading v-if="isLoading"></h-loading>
</template>
  <script>
import HConfig from "@/js/base/config";
import HInput from "@/components/input/HInput.vue";
import {
  getFavouritePostsFromLocalStorage,
  addFavouritePostIdToLocalStorage,
  removeFavouritePostIdFromLocalStorage,
  getUserFromLocalStorage,
} from "@/stores/localStorage.js";
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
    if (this.User) {
      await this.getListIdFavouritePosts();
      this.newComment.PostID = this.id;
      this.newComment.UserID = this.User.UserID;
      this.newComment.Content = null;
    } else {
      this.newComment.Content = "Đăng nhập để gửi bình luận";
    }
  },
  methods: {
    async onClickFavouritePosts(event) {
      event.preventDefault();
      if (this.User) {
        this.favouritePosts();
      } else {
        this.$router.push("/dang-nhap");
      }
    },
    async onClickUnFavouritePosts(event) {
      event.preventDefault();
      this.unFavouritePosts();
    },
    async getListIdFavouritePosts() {
      this.listIdFavouritePosts = await getFavouritePostsFromLocalStorage();
    },
    favouritePosts() {
      var url = this.HConfig.API.FavouritePosts;
      this.axios
        .post(url, {
          UserID: this.User.UserID,
          PostID: this.Post.PostID,
        })
        .then((response) => {
          if (response.data.Success) {
            this.Post.NumberFavourite++;
            addFavouritePostIdToLocalStorage(this.Post.PostID);
            this.getListIdFavouritePosts();
          } else {
            this.errorMessage = response.data.UserMsg;
          }
        })
        .catch(() => {
          this.errorMessage = this.HResource.Message.Exception;
        });
    },
    unFavouritePosts() {
      var url = `https://localhost:9999/api/v1/FavouritePosts?userId=${this.User.UserID}&postId=${this.Post.PostID}`;
      this.axios
        .delete(url)
        .then((response) => {
          if (response.data.Success) {
            this.Post.NumberFavourite--;
            removeFavouritePostIdFromLocalStorage(this.Post.PostID);
            this.getListIdFavouritePosts();
          } else {
            this.errorMessage = response.data.UserMsg;
          }
        })
        .catch(() => {
          this.errorMessage = this.HResource.Message.Exception;
        });
    },
    addComment() {
      if (this.newComment.Content) {
        console.log(this.newComment);
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

    /**
     * Sự kiện cho component Dialog
     * Author: Nguyễn Văn Huy(01/04/2023)
     */
    dialogEvent(key) {
      switch (key) {
        case "close":
          this.errorMessage = null;
          break;
        case "cancel":
          this.errorMessage = null;
          this.dialogType = this.HEnum.DialogType.Error;
          break;
        case "no":
          this.errorMessage = null;
          this.dialogType = this.HEnum.DialogType.Error;
          break;
        case "yes":
          this.deletePost();
          this.dialogType = this.HEnum.DialogType.Error;
          break;
        default:
          break;
      }
    },

    onClickDelete() {
      this.dialogType = this.HEnum.DialogType.Warning;
      this.errorMessage = "Bạn có chắc muốn xoá tin đăng này không?";
    },

    deletePost() {
      try {
        this.axios.delete(this.HConfig.API.Posts + this.id).then((response) => {
          if (response.data.Success) {
            this.$router.push("/quan-ly-tin-dang");
          } else {
            this.dialogType = this.HEnum.DialogType.Error;
            this.errorMessage = response.data.UserMsg;
          }
        });
      } catch (error) {
        this.dialogType = this.HEnum.DialogType.Error;
        this.errorMessage = error;
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
      listIdFavouritePosts: [],
      URL: HConfig.URL,
      User: {},
      Post: {},
      Seller: {},
      Comments: [],
      newComment: {},
      ImageID: null,
      isLoading: true,
      showDialog: false,
      errorMessage: null,
    };
  },
};
</script>

<style>
@import url(@/css/views/thepostdetail.css);
</style>