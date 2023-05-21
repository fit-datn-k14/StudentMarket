<template>
  <div class="single-product mt-150 mb-150">
    <div class="container">
      <div class="row">
        <div class="col-md-4">
          <div class="btnUploadImage" @click="onClickLoadAvatar">
            Tải ảnh
            <input
              type="file"
              ref="fileInput"
              @change="LoadAvatar"
              accept="image/jpeg, image/png"
            />
          </div>
          <div class="single-product-img">
            <img
              v-if="previewUrl"
              class="cud_avatar border border-dark"
              :src="previewUrl"
            />
            <img
              v-else
              class="cud_avatar border border-dark"
              :src="URL + `Images/users/avatar/${User.Avatar}`"
            />
          </div>
        </div>
        <div class="col-md-8 row">
          <h3>Thay Đổi Thông Tin Cá Nhân</h3>
          <div class="col-md-6 cud__column">
            <h-input
              ref="txtUserName"
              label="Họ Tên"
              :required="true"
              v-model="User.FullName"
            />
            <div class="row">
              <h-date-picker
                class="DateOfBirth"
                label="Ngày sinh"
                v-model="User.DateOfBirth"
                :hasValidate="true"
              ></h-date-picker>
              <div class="col-md-6 input-wrapper item-gt">
                <label for="radioGender">Giới tính</label>
                <div ref="radioGender" class="btn-radio">
                  <div class="btn-radio__option">
                    <input
                      type="radio"
                      name="gender"
                      :value="0"
                      v-model="User.Gender"
                    />
                    <label for="female">Nữ</label>
                    <div></div>
                  </div>
                  <div class="btn-radio__option">
                    <input
                      type="radio"
                      name="gender"
                      :value="1"
                      v-model="User.Gender"
                    />
                    <label for="male">Nam</label>
                    <div></div>
                  </div>
                  <div class="btn-radio__option">
                    <input
                      type="radio"
                      name="gender"
                      :value="2"
                      v-model="User.Gender"
                    />
                    <label for="other">Khác</label>
                    <div></div>
                  </div>
                </div>
              </div>
            </div>
            <h-combobox
              ref="cbbLocations"
              label="Khu vực"
              :api="apiGetLocations"
              :defaultItem="defaultLocation"
              propText="LocationName"
              propValue="LocationID"
              v-model="User.LocationID"
            ></h-combobox>
            <h-input ref="txtAddress" label="Địa Chỉ" v-model="User.Address" />
          </div>
          <div class="col-md-6 cud__column">
            <h-input
              ref="txtPhoneNumber"
              label="Số Điện Thoại:"
              v-model="User.PhoneNumber"
            />
            <h-input ref="txtEmail" label="Email:" v-model="User.Email" />
            <div class="row">
              <h-input
                class="col-md-6"
                ref="txtIndentityCard"
                label="Số CMND"
                title="Số chứng minh nhân dân"
                v-model="User.IdentityNumber"
              />
              <div class="textfield col-md-6">
                <h-date-picker
                  label="Ngày Cấp"
                  v-model="User.IdentityDate"
                  :hasValidate="true"
                ></h-date-picker>
              </div>
            </div>
            <h-input
              ref="txtIdentityPlace"
              label="Nơi cấp"
              v-model="User.IdentityPlace"
            />
          </div>

          <HButton
            class="circle btnChangeInfo"
            ref="btnChangeInfo"
            value="Thay đổi Thông Tin Cá Nhân"
            type="btn-pri"
            @click="updateUser"
          />
        </div>
      </div>
    </div>
  </div>
  <h-loading v-if="isLoading"></h-loading>
</template>
      <script>
import HConfig from "@/js/base/config";
import { eventBus } from "@/js/eventbus";
import {
  saveUserToLocalStorage,
  getUserFromLocalStorage,
} from "@/stores/localStorage.js";
export default {
  name: "ChangeUserInfo",
  props: {
    id: {
      type: String,
      required: true,
    },
  },
  async created() {
    this.User = getUserFromLocalStorage();
    this.User.DateOfBirth = this.HCommon.formatDate(this.User.DateOfBirth);
    this.ImageID = this.Post.ImageName;
    this.newComment.PostID = this.id;
    this.newComment.UserID = this.User.UserID;
  },
  methods: {
    onClickLoadAvatar() {
      this.$refs.fileInput.click();
    },

    LoadAvatar(event) {
      this.previewFile = event.target.files[0];
      const reader = new FileReader();
      reader.onload = (e) => {
        this.previewUrl = e.target.result;
      };
      reader.readAsDataURL(this.previewFile);
    },

    updateUser() {
      // create a FormData object to store the post data
      //this.onValidate();
      if (!this.errorMessage) {
        const formData = new FormData();
        formData.append("User.UserID", this.User.UserID);
        formData.append("User.UserName", this.User.UserName);
        formData.append("User.FullName", this.User.FullName);
        if (this.User.DateOfBirth) {
          formData.append("User.DateOfBirth", this.User.DateOfBirth);
        }
        if (this.User.Gender != null) {
          formData.append("User.Gender", this.User.Gender);
        }
        if (this.User.LocationID) {
          formData.append("User.LocationID", this.User.LocationID);
        }
        if (this.User.Address) {
          formData.append("User.Address", this.User.Address);
        }
        if (this.User.PhoneNumber) {
          formData.append("User.PhoneNumber", this.User.PhoneNumber);
        }
        if (this.User.Email) {
          formData.append("User.Email", this.User.Email);
        }
        if (this.User.IdentityNumber) {
          formData.append("User.IdentityNumber", this.User.IdentityNumber);
        }
        if (this.User.IdentityDate) {
          formData.append("User.IdentityDate", this.User.IdentityDate);
        }
        if (this.User.IdentityPlace) {
          formData.append("User.IdentityPlace", this.User.IdentityPlace);
        }
        formData.append("User.Role", this.User.Role);

        if (this.previewFile) {
          formData.append("Image", this.previewFile);
        } else {
          formData.append("User.Avatar", this.User.Avatar);
        }

        this.axios
          .put("https://localhost:9999/api/v1/Users/", formData, {
            headers: {
              "Content-Type": "multipart/form-data",
            },
          })
          .then((response) => {
            if (response.data.Success) {
              saveUserToLocalStorage(response.data.Data);
              eventBus.emit("updateUser");
              this.$router.push(`cai-dat-tai-khoan`);
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
      URL: HConfig.URL,
      User: {},
      Post: {},
      Seller: {},
      Comments: [],
      newComment: {},
      ImageID: null,
      previewFile: null,
      previewUrl: null,
      isLoading: false,
      apiGetLocations: "https://localhost:9999/api/v1/Locations",
      defaultLocation: {
        LocationID: null,
        LocationName: "Chọn khu vực",
      },
    };
  },
};
</script>
    
    <style >
.btnUploadImage {
  font-size: 16px;
  color: var(--primary-color);
  float: right;
  cursor: pointer;
  text-decoration: underline;
}

.cud_avatar {
  width: 100%;
  aspect-ratio: 1 / 1; /* thiết lập tỷ lệ khung hình 1:1 */
  border-radius: 50%; /* làm tròn góc hình ảnh */
  object-fit: cover;
}

.btnUploadImage > input {
  display: none;
}

.btnChangeInfo input {
  float: right;
}

.DateOfBirth {
  width: 156px !important;
}

.cud__column > div {
  padding-bottom: 16px;
}

.addCommentBox {
  display: flex;
  padding-top: 12px;
}

.listcomments {
  display: flex;
  flex-direction: column;
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
.single-product-describe {
  min-height: 180px;
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
}

.single-product h3 {
  font-size: 28px;
  font-weight: 600;
  margin-bottom: 20px;
}

.single-product-pricing {
  font-size: 24px;
  font-weight: 600;
  margin-bottom: 30px;
  color: var(--red-color);
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
</style>