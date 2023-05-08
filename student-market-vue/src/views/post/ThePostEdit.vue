<template>
  <div class="the-post-post">
    <div class="tpp__container">
      <div class="tpp__left">
        <h5 class="title__img">Ảnh Sản Phẩm</h5>
        <div class="listImages" v-if="previewUrls.length">
          <div v-for="(url, index) in previewUrls" :key="index">
            <span class="removeImg" @click="removeImg(index)">&times;</span>
            <img :src="url" />
          </div>
        </div>
        <input
          type="file"
          ref="fileInput"
          @change="previewImages"
          multiple
          accept="image/jpeg, image/png"
        />
        <div class="inputImage" @click="onClickFileInput"></div>
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
              :defaultItem="defaultLocation"
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
            value="Đăng Tin"
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
  created() {
    this.user = getUserFromLocalStorage();
    this.postDetail.UserID = this.user.UserID;
    this.postDetail.LocationID = this.user.LocationID;
  },
  methods: {
    onClickFileInput() {
      this.$refs.fileInput.click();
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

    previewImages(event) {
      this.previewFiles = event.target.files;
      const files = event.target.files;
      if (files.length > 6) {
        alert("Bạn chỉ được tải lên tối đa 6 ảnh!");
        return;
      }
      for (let i = 0; i < files.length; i++) {
        const reader = new FileReader();
        reader.onload = (e) => {
          if (
            !this.previewUrls.includes(e.target.result) &&
            this.previewUrls.length < 6
          ) {
            this.previewUrls.push(e.target.result);
          } else {
            alert("Bạn chỉ được tải lên tối đa 6 ảnh!");
            return;
          }
        };
        reader.readAsDataURL(files[i]);
      }
    },
    removeImg(index) {
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

        formData.append("Post.Title", this.postDetail.Title);
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

        for (let i = 0; i < this.previewFiles.length; i++) {
          formData.append("Images", this.previewFiles[i]);
        }

        axios
          .post("https://localhost:9999/api/v1/Posts/", formData, {
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
      user: {},
      errorMessage: null,
      previewUrls: [],
      previewFiles: [],
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