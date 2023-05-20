<template>
  <div class="the-post-post">
    <div class="tpp__container">
      <div class="tpp__left">
        <h5 class="title__img">Ảnh Sản Phẩm</h5>
        <div
          v-if="
            postDetail.ListImages &&
            previewUrls.length + postDetail.ListImages.length <= 6
          "
          class="btnUploadImage"
          @click="onClickFileInput"
        >
          Thêm ảnh
          <input
            type="file"
            ref="fileInput"
            @change="addImage"
            accept="image/jpeg, image/png"
          />
        </div>
        <div class="listImages">
          <div
            v-for="(image, indexImg) in postDetail.ListImages"
            :key="indexImg"
          >
            <span class="removeImg" @click="removeOldImg(indexImg)"
              >&times;</span
            >
            <img :src="URL + `Images/posts/${id}/${image}`" />
          </div>
          <div v-for="(url, index) in previewUrls" :key="index">
            <span class="removeImg" @click="removeNewImg(index)">&times;</span>
            <img :src="url" />
          </div>
        </div>
        <img
          v-if="postDetail.ListImages && postDetail.ListImages.length > 0"
          class="ppd__img border border-dark"
          :src="URL + `Images/posts/${id}/${postDetail.ListImages[0]}`"
        />
        <img
          v-else-if="previewUrls && previewUrls.length > 0"
          class="ppd__img border border-dark"
          :src="previewUrls[0]"
        />
        <img
          v-else
          class="ppd__img border border-dark"
          src="@/assets/img/noimage.jpg"
        />
      </div>
      <div class="tpp__right">
        <div class="form-group">
          <h5 class="title__detail">Thông Tin Chi Tiết</h5>
        </div>

        <div class="row">
          <div class="form-group col-md-7">
            <h-input
              ref="txtTitle"
              label="Tên Sản Phẩm"
              :required="true"
              v-model="postDetail.Title"
            />
          </div>
          <div class="form-group col-md-5">
            <h-input
              ref="txtPrice"
              type="Number"
              label="Giá"
              v-model="postDetail.Price"
            />
          </div>
        </div>
        <div class="row">
          <div class="form-group col-md-6">
            <h-combobox
              ref="cbbCategories"
              label="Danh mục"
              :required="true"
              :api="apiGetCategories"
              :defaultItem="defaultCategory"
              propText="CategoryName"
              propValue="CategoryID"
              v-model="postDetail.CategoryID"
            ></h-combobox>
          </div>
          <div class="form-group col-md-6">
            <h-combobox
              ref="cbbLocations"
              label="Khu vực"
              :required="true"
              :api="apiGetLocations"
              propText="LocationName"
              propValue="LocationID"
              v-model="postDetail.LocationID"
            ></h-combobox>
          </div>
        </div>
        <div class="form-group">
          <h-input
            ref="txtAddress"
            label="Địa Chỉ"
            :required="false"
            v-model="postDetail.Address"
          />
        </div>
        <div class="form-group">
          <h-input
            ref="txtDescribe"
            label="Mô Tả"
            type="textarea"
            v-model="postDetail.PostDescribe"
          />
        </div>
        <div class="form-group">
          <HButton
            class="circle"
            ref="btnDangTin"
            value="Lưu thay đổi"
            type="btn-pri"
            :disabled="false"
            @click="submitPostData"
          />
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
  <h-toast ref="toast" />
</template>
  
  <script>
import { getUserFromLocalStorage } from "@/stores/localStorage";
import axios from "axios";
export default {
  name: "ThePostPost",
  props: {
    id: {
      type: String,
      required: true,
    },
  },
  async created() {
    this.user = await getUserFromLocalStorage();
    await this.loadData();
  },
  methods: {
    async loadData() {
      this.isLoading = true;
      try {
        var url = this.HConfig.API.Posts + this.id;
        await this.axios.get(url).then((response) => {
          if (response.data.Success) {
            this.postDetail = response.data.Data;
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

    onClickFileInput() {
      this.$refs.fileInput.click();
    },
    addImage(event) {
      const files = event.target.files;
      for (let i = 0; i < files.length; i++) {
        const file = files[i];
        const reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = () => {
          this.previewUrls.push(reader.result);
        };
        this.previewFiles.push(file);
      }
    },

    /**
     * Sự kiện cho component Dialog
     * Author: Nguyễn Văn Huy(01/04/2023)
     */
    dialogEvent(key) {
      this.errorMessage = null;
      switch (key) {
        case "close":
          break;
        default:
          break;
      }
    },
    removeOldImg(index) {
      this.imagesDel.push(this.postDetail.ListImages[index]);
      this.postDetail.ListImages.splice(index, 1);
    },
    removeNewImg(index) {
      this.previewUrls.splice(index, 1);
    },

    /**
     * Kiểm tra dữ liệu hợp lệ
     * Author: Nguyễn Văn Huy (30/03/2023)
     */
    onValidate() {
      if (!this.postDetail.Title) {
        this.errorMessage = "Tiêu đề không được bỏ trống";
        this.$refs.txtTitle.onValidate();
      }

      if (!this.postDetail.CategoryID) {
        this.errorMessage = "Danh mục không được bỏ trống";
        this.$refs.cbbCategories.onValidate();
      }

      if (!this.postDetail.LocationID) {
        this.errorMessage = "Khu vực không được bỏ trống";
        this.$refs.cbbLocations.onValidate();
      }
    },
    submitPostData() {
      // create a FormData object to store the post data
      this.onValidate();

      if (!this.errorMessage) {
        const formData = new FormData();

        formData.append("Post.PostID", this.postDetail.PostID);
        formData.append("Post.Title", this.postDetail.Title);
        console.log(this.postDetail.Price);
        if (this.postDetail.Price) {
          formData.append("Post.Price", this.postDetail.Price);
        }
        formData.append("Post.CategoryID", this.postDetail.CategoryID);
        formData.append("Post.LocationID", this.postDetail.LocationID);
        formData.append("Post.UserID", this.postDetail.UserID);
        if (this.postDetail.Address) {
          formData.append("Post.Address", this.postDetail.Address);
        }
        if (this.postDetail.PostDescribe) {
          formData.append("Post.PostDescribe", this.postDetail.PostDescribe);
        }
        for (let i = 0; i < this.imagesDel.length; i++) {
          formData.append("ImagesDel", this.imagesDel[i]);
        }

        if (this.postDetail.ListImages.length > 0) {
          formData.append("Post.ImageName", this.postDetail.ListImages[0]);
        }

        for (let i = 0; i < this.previewFiles.length; i++) {
          formData.append("Images", this.previewFiles[i]);
        }

        var url = this.URL + `Posts/${this.id}`;
        axios
          .put(url, formData, {
            headers: {
              "Content-Type": "multipart/form-data",
            },
          })
          .then((response) => {
            if (response.data.Success) {
              var routerPost = `/post/${response.data.Data.PostID}`;
              this.$router.push(routerPost);
            }
          })
          .catch(() => {
            this.errorMessage = this.HResource.Message.Exception;
          });
      }
    },
  },
  data() {
    return {
      URL: this.HConfig.URL,
      user: {},
      errorMessage: null,
      previewUrls: [],
      previewFiles: [],
      imagesDel: [],
      apiGetCategories: "https://localhost:9999/api/v1/Categories",
      apiGetLocations: "https://localhost:9999/api/v1/Locations",
      defaultCategory: {
        CategoryID: null,
        CategoryName: "Chọn danh mục",
      },
      defaultLocation: {
        LocationID: null,
        LocationName: "Chọn khu vực",
      },
      postDetail: {},
    };
  },
};
</script>
  
  <style>
.the-post-post {
  display: flex;
  justify-content: center;
}
.tpp__container {
  display: flex;
  column-gap: 24px;
  margin-top: 24px;
  width: 900px;
  height: 80vh;
  border-radius: 8px;
  background-color: #fff;
  padding: 16px;
}

.tpp__left {
  display: flex;
  flex-direction: column;
  row-gap: 12px;
  width: 300px;
  height: 100%;
}

.title__img,
.title__detail {
  font-weight: bold;
}

.tpp__left input {
  display: none;
}

.removeImg {
  cursor: pointer;
  width: 20px;
  height: 20px;
  color: #fff;
  background-color: #000;
  text-align: center;
  border-radius: 10px;
  position: absolute;
  top: -10px;
  right: -10px;
}

.listImages {
  width: 100%;
  display: flex;
  column-gap: 5%;
  row-gap: 12px;
  flex-wrap: wrap;
}

.listImages div {
  position: relative;
  width: 30%;
  height: 90px;
  border: 1px solid var(--grey-color);
  border-radius: 4px;
}

.listImages img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  object-position: center;
}

.inputImage {
  cursor: pointer;
  content: none;
  background: #ccc;
  border: 1px solid var(--primary-color);
  border-radius: 4px;
  width: 100%;
  height: 240px;
  background: url(@/assets/img/add-img.jpg);
  background-size: cover;
  background-position: center;
}

.tpp__right {
  display: flex;
  flex-direction: column;
  row-gap: 12px;
  width: 300px;
  height: 100%;
  flex-grow: 1;
}
</style>