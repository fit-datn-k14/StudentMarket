<template>
  <div ref="formUserDetail" class="popup_add_user" @keydown="keydownDetail">
    <div class="bgshadow" tabindex="0"></div>
    <div class="popup" tabindex="0">
      <div class="icon-top-right-popup">
        <h-icon
          class="icon popup__btn-close"
          position="-89px -145px"
          size="22 22"
          title="Hỗ trợ"
        />
        <h-icon
          class="icon popup__btn-close"
          position="-147px -147px"
          size="18 18"
          title="Đóng(ESC)"
          @click="onClickClose"
        />
      </div>
      <div class="popup__title">
        <div class="popup__title-text">{{ this.title }}</div>
      </div>
      <div class="popup__content">
        <div class="form-info">
          <div class="form-group form-info-left">
            <div class="m-row">
              <h-input
                ref="txtUserName"
                label="Tài khoản"
                :required="true"
                v-model="User.UserName"
                :disabled="UserInput.UserID"
              />
              <h-input
                ref="txtPassword"
                label="Mật khẩu"
                :required="true"
                type="password"
                v-model="User.Password"
              />
            </div>
            <div class="m-row">
              <h-input
                ref="txtFullName"
                label="Họ   Tên"
                :required="true"
                v-model="User.FullName"
              />
              <div class="textfield">
                <h-combobox
                  ref="cbbLocations"
                  :api="apiGetLocations"
                  label="Khu vực"
                  propText="LocationName"
                  propValue="LocationID"
                  v-model="User.LocationID"
                ></h-combobox>
              </div>
            </div>
            <div class="input-wrapper item-gt">
              <label for="radioGender">Vai trò</label>
              <div ref="radioGender" class="btn-radio">
                <div class="btn-radio__option">
                  <input
                    type="radio"
                    name="role"
                    :value="0"
                    v-model="this.User.Role"
                  />
                  <label for="user">Người dùng</label>
                  <div></div>
                </div>

                <div class="btn-radio__option">
                  <input
                    type="radio"
                    name="role"
                    :value="1"
                    v-model="this.User.Role"
                  />
                  <label for="censor">Kiểm duyệt</label>
                  <div></div>
                </div>
                <div class="btn-radio__option">
                  <input
                    type="radio"
                    name="role"
                    :value="2"
                    v-model="this.User.Role"
                  />
                  <label for="censor">Admin</label>
                  <div></div>
                </div>
              </div>
            </div>
          </div>
          <div class="form-group form-info-right">
            <div class="m-row">
              <h-date-picker
                label="Ngày sinh"
                v-model="User.DateOfBirth"
                :hasValidate="true"
              ></h-date-picker>
              <div class="input-wrapper item-gt">
                <label for="radioGender">Giới tính</label>
                <div ref="radioGender" class="btn-radio">
                  <div class="btn-radio__option">
                    <input
                      type="radio"
                      name="gender"
                      :value="0"
                      v-model="this.User.Gender"
                    />
                    <label for="female">Nữ</label>
                    <div></div>
                  </div>

                  <div class="btn-radio__option">
                    <input
                      type="radio"
                      name="gender"
                      :value="1"
                      v-model="this.User.Gender"
                    />
                    <label for="male">Nam</label>
                    <div></div>
                  </div>
                  <div class="btn-radio__option">
                    <input
                      type="radio"
                      name="gender"
                      :value="2"
                      v-model="this.User.Gender"
                    />
                    <label for="other">Khác</label>
                    <div></div>
                  </div>
                </div>
              </div>
            </div>
            <div class="second-row">
              <h-input
                ref="txtIndentityCard"
                label="Số CMND"
                title="Số chứng minh nhân dân"
                v-model="User.IdentityNumber"
              />
              <div class="textfield">
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
          <div class="form-group form-info-bottom">
            <div class="form-group-row">
              <h-input
                ref="txtAddress"
                label="Địa chỉ"
                v-model="User.Address"
              />
            </div>
            <div class="form-group-row">
              <div class="item-contact">
                <h-input
                  ref="txtPhoneNumber"
                  label="Số điện thoại"
                  title="Số điện thoại di động"
                  v-model="User.PhoneNumber"
                />
              </div>
              <div class="item-contact">
                <h-input
                  ref="txtEmail"
                  label="Email"
                  v-model="User.Email"
                ></h-input>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div class="popup__footer">
        <div>
          <HButton
            type="btn-second"
            id="btnStore"
            value="Cất"
            title="Cất (Ctrl + S)"
            @click="updateUser(false)"
          />
          <HButton
            type="btn-pri"
            id="btnStoreAndAdd"
            value="Cất và Thêm"
            title="Cất và Thêm (Ctrl + Shift + S)"
            @click="updateUser(true)"
          ></HButton>
        </div>
        <div>
          <HButton
            type="btn-second"
            @click="closePopup"
            ref="btnCancel"
            value="Huỷ"
            @keydown.tab="focusFirstInput"
          />
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
  </div>
</template>

<script>
export default {
  name: "UserDetail",
  props: {
    modelValue: [Object],
    UserInput: {
      type: Object,
      required: false,
    },
    title: {
      type: String,
      required: false,
    },
  },
  async created() {
    this.UserRoot = await JSON.stringify(this.UserInput);
    this.oldPassword = this.UserInput.Password;
    this.User = await JSON.parse(this.UserRoot);
    if (!this.UserInput.UserID) {
      await this.GetNewCode();
    }
    if (this.User.DateOfBirth)
      this.User.DateOfBirth = this.HCommon.formatDate(this.User.DateOfBirth);
    if (this.User.IdentityDate)
      this.User.IdentityDate = this.HCommon.formatDate(this.User.IdentityDate);
    this.UserRoot = JSON.stringify(this.User);
  },
  mounted() {
    this.focusFirstInput();
  },
  methods: {
    /**
     * Sự kiện nhấn phím tắt cho form chi tiết
     * Author: huynv (02/03/2023)
     */
    keydownDetail(event) {
      event.stopPropagation();
      if (event.key === "Escape") {
        this.onClickClose();
      } else if (event.ctrlKey && event.shiftKey && event.keyCode === 83) {
        this.updateUser(true);
      } else if (event.ctrlKey && event.keyCode === 83) {
        event.preventDefault();
        this.updateUser(false);
      }
    },
    /**
     * Đóng popup
     * Author: huynv (02/03/2023)
     */
    closePopup() {
      this.$emit("eventDetail");
    },

    /**
     * Định dạng dữ liệu kiểu Date
     * Author: huynv (27/03/2023)
     */
    onChangeDate() {
      if (this.User.DateOfBirth) {
        this.User.DateOfBirth = this.HCommon.formatDate(this.User.DateOfBirth);
      } else {
        this.User.DateOfBirth = null;
      }
      if (this.User.IdentityDate) {
        this.User.IdentityDate = this.HCommon.formatDate(
          this.User.IdentityDate
        );
      } else {
        this.User.IdentityDate = null;
      }
    },

    /**
     * Sự kiện cho component Dialog
     * Author: Nguyễn Văn Huy(01/04/2023)
     */
    dialogEvent(key) {
      this.errorMessage = null;
      this.focusFirstInput();
      switch (key) {
        case "close":
          break;
        case "cancel":
          this.dialogType = this.HEnum.DialogType.Error;
          break;
        case "no":
          this.closePopup();
          break;
        case "yes":
          this.updateUser(false);
          break;
        default:
          break;
      }
    },

    /**
     * Sự kiện khi click nút đóng
     * Author: Nguyễn Văn Huy(30/03/2023)
     */
    onClickClose() {
      var UserNew = JSON.stringify(this.User);
      if (UserNew === this.UserRoot) {
        this.closePopup();
      } else {
        this.errorMessage = "Dữ liệu có sự thay đổi. Bạn có muốn cất không?";
        this.dialogType = this.HEnum.DialogType.Confirm;
      }
    },

    /**
     * Làm mới Popup
     * Author: huynv (04/03/2023)
     */
    async clearPopup() {
      this.User = {};
      this.focusFirstInput();
      await this.GetNewCode();
      this.UserRoot = JSON.stringify(this.User);
    },

    /**
     * Focus mã
     * Author: huynv (04/03/2023)
     */
    focusFirstInput(event) {
      if (event) {
        event.preventDefault();
      }
      this.$refs.txtUserName.$refs.hinput.focus();
    },

    /**
     * Kiểm tra dữ liệu hợp lệ
     * Author: Nguyễn Văn Huy (30/03/2023)
     */
    onValidate() {
      if (!this.User.FullName) {
        this.errorMessage = this.HResource.Message.FullNameEmpty;
        this.$refs.txtFullName.onValidate();
      }

      if (!this.User.UserName) {
        this.errorMessage = this.HResource.Message.UserNameEmpty;
        this.$refs.txtUserName.onValidate();
      }

      if (this.User.DateOfBirth) {
        if (new Date(this.User.DateOfBirth) > new Date()) {
          this.errorMessage = this.HResource.Message.DateOfBirthBiggerNow;
        } else {
          this.User.DateOfBirth = this.HCommon.formatDate(
            this.User.DateOfBirth
          );
        }
      } else {
        delete this.User.DateOfBirth;
      }

      if (this.User.IdentityDate) {
        if (new Date(this.User.IdentityDate) > new Date()) {
          this.errorMessage = this.HResource.Message.IdentityDateBiggerNow;
        } else {
          this.User.IdentityDate = this.HCommon.formatDate(
            this.User.IdentityDate
          );
        }
      } else {
        delete this.User.IdentityDate;
      }

      if (this.errorMessage) {
        this.dialogType = this.HEnum.DialogType.Error;
      }
    },

    /**
     * Thêm hoặc sửa bản ghi
     * Author: Văn Huy (16/03/2023)
     */
    async updateUser(addContinue) {
      var e = this.User;
      if (e.Password == this.oldPassword) {
        e.Password = null;
      }
      var url = this.HConfig.API.Users;
      await this.onValidate();
      if (!this.errorMessage) {
        if (e.UserID) {
          url = url + e.UserID;
          try {
            await this.axios.put(url, e).then((response) => {
              if (response.data.Success) {
                this.$emit("eventDetail", "showToast", response.data.UserMsg);
                if (addContinue) {
                  this.clearPopup();
                  this.$emit("eventDetail", "toTitleAdd");
                  this.$emit("eventDetail", "refresh");
                } else {
                  this.$emit("eventDetail", "refresh");
                  this.closePopup();
                }
              } else {
                this.errorMessage = response.data.UserMsg;
              }
            });
          } catch (error) {
            this.errorMessage = this.HResource.Message.Exception;
          }
        } else {
          try {
            await this.axios.post(url, this.User).then((response) => {
              if (response.data.Success) {
                this.$emit("eventDetail", "showToast", response.data.UserMsg);
                if (addContinue) {
                  this.clearPopup();
                  this.$emit("eventDetail", "toTitleAdd");
                  this.$emit("eventDetail", "refresh");
                } else {
                  this.$emit("eventDetail", "refresh");
                  this.closePopup();
                }
              } else {
                this.errorMessage = response.data.UserMsg;
              }
            });
          } catch (error) {
            this.errorMessage = this.HResource.Message.Exception;
          }
        }
      }
    },

    showErrorMessage(str) {
      this.errorMessage = str;
    },
  },
  data() {
    return {
      oldPassword: null,
      isLoading: false,
      dialogType: this.HEnum.DialogType.Error,
      errorMessage: "",
      UserRoot: "",
      User: {},
      apiGetLocations: this.HConfig.API.Locations,
    };
  },
};
</script>

<style scoped>
.icon-top-right-popup {
  position: absolute;
  top: 0;
  right: 0;
  display: flex;
  column-gap: 8px;
}
.popup_add_user {
  position: fixed;
  top: 0;
  right: 0;
  bottom: 0;
  left: 0;
  z-index: 4;
}

.bgshadow {
  display: block;
  position: fixed;
  top: 0;
  right: 0;
  bottom: 0;
  left: 0;
  background-color: #00000050;
  z-index: 10;
}

.popup {
  width: 900px;
  position: fixed;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  background-color: #fff;
  border-radius: 4px;
  min-width: 360px;
  z-index: 10;
}

.popup * {
  font-family: Roboto;
}

.popup__btn-close {
  background: url(@/assets/img/Sprites.64af8f61.svg) no-repeat -143px -145px;
  position: absolute;
  width: 24px;
  height: 24px;
  border: none;
  top: 24px;
  right: 24px;
  cursor: pointer;
}

.popup__btn-help {
  background: url(@/assets/img/Sprites.64af8f61.svg) no-repeat -88px -145px;
  width: 24px;
  height: 24px;
  position: absolute;
  width: 24px;
  height: 24px;
  border: none;
  top: 24px;
  right: 60px;
  cursor: pointer;
}

.popup__title {
  padding: 24px 24px 0;
  box-sizing: border-box;
}

.popup__title-text {
  font-size: 24px;
  font-weight: 700;
}

.popup__content {
  padding: 24px;
  box-sizing: border-box;
}

.popup__footer {
  display: flex;
  flex-direction: row-reverse;
  justify-content: space-between;
  align-items: center;
  width: 100%;
  height: 60px;
  background-color: #f5f5f5;
  border-radius: 0 0 4px 4px;
  padding: 24px;
  box-sizing: border-box;
}

.popup__footer > div:first-child {
  display: flex;
  align-items: center;
  column-gap: 8px;
}

.popup__content {
  display: flex;
}

/* 
------------------------------------------------ */

.popup-add-employee {
  font-family: "Roboto";
}

.popup__text {
  width: 360px;
}

.ctr-item {
  width: 20%;
}

.form-info {
  width: 100%;
  display: flex;
  justify-content: space-between;
  flex-wrap: wrap;
  row-gap: 14px;
}

.form-group-row + .form-group-row {
  margin-top: 14px;
}

.form-group-row {
  display: flex;
  column-gap: 8px;
}

.form-info-left {
  display: flex;
  flex-direction: column;
  row-gap: 14px;
  width: calc(50% - 16px);
}

.form-info-right {
  display: flex;
  flex-direction: column;
  row-gap: 14px;
  width: calc(50% - 16px);
}

.form-info-bottom {
  width: 100%;
}

.m-row {
  display: flex;
  column-gap: 8px;
}

.m-row > div:first-child {
  width: 50%;
}

.second-row > div:first-child {
  width: 60%;
}

.second-row > div:last-child {
  width: 40%;
}

.form-info-right > .second-row {
  display: flex;
  align-items: flex-start;
  column-gap: 8px;
}

.m-row > div:last-child {
  flex: 1;
}

.item-contact {
  width: 25%;
}

.department {
  width: 100%;
}

.paging__combobox {
  width: 220px;
}

.paging__combobox > .rotate-180 {
  transform: rotate(-180deg);
}

.form-checkbox {
  display: flex;
  align-items: center;
  column-gap: 8px;
  position: absolute;
  top: 28px;
  left: 200px;
}

.popup__title {
  display: flex;
  align-items: center;
  column-gap: 8px;
}

.popup__title .checkbox {
  margin-left: 8px;
}

.popup__title label {
  font-size: 14px;
  font-weight: 500;
}

.table__row:has(.abc) > td {
  z-index: 111111 !important;
}

.searchbox:has(input:focus) {
  border-color: #50b83c;
}

.delete-popup {
  width: 450px;
  box-shadow: 0 0 4px rgba(23, 27, 42, 0.24);
}
</style>