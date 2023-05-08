<template>
  <div class="register">
    <div class="row justify-content-center">
      <div class="col-md-6 row g-lg-3">
        <h2>Thay Đổi Mật Khẩu</h2>
        <h-input
          ref="txtOldPassword"
          label="Mật khẩu hiện tại"
          type="password"
          :required="true"
          v-model="newAccount.Password"
        />
        <h-input
          ref="txtNewPassword"
          label="Mật khẩu mới"
          type="password"
          :required="true"
          v-model="newAccount.NewPassword"
        />
        <h-input
          ref="txtNewPasswordConfirm"
          label="Nhập lại mật khẩu mới"
          type="password"
          :required="true"
          v-model="newAccount.PasswordConfirm"
        />
        <div class="errorMessage">{{ errorMessage }}</div>
        <HButton
          class="btnChangePassword"
          type="btn-pri"
          ref="btnChangePassword"
          value="Đổi mật khẩu"
          title="Đổi mật khẩu"
          @click="changePassword"
        ></HButton>
      </div>
    </div>
  </div>
</template>
  <script>
import { eventBus } from "@/js/eventbus";
import axios from "axios";
import {
  saveUserToLocalStorage,
  getUserFromLocalStorage,
} from "@/stores/localStorage";
export default {
  name: "ChangePassword",
  data() {
    return {
      newAccount: {},
      user: {},
      errorMessage: "",
      regexFullName: "/^[a-zA-Z _-]+$/",
    };
  },
  created() {
    this.user = getUserFromLocalStorage();
    this.newAccount.UserName = this.user.UserName;
  },
  methods: {
    login(user) {
      axios
        .post("https://localhost:9999/api/v1/Users/Login", {
          UserName: user.UserName,
          Password: user.NewPassword,
        })
        .then((response) => {
          if (response.data.Success) {
            // Lưu thông tin tài khoản vào localStorage
            var user = response.data.Data;
            saveUserToLocalStorage(user);
            eventBus.emit("login");
            // Chuyển hướng đến trang chủ
            this.$router.push("/trang-chu");
          } else {
            this.errorMessage = response.data.UserMsg;
          }
        })
        .catch(() => {
          this.errorMessage = this.HResource.Message.Exception;
        });
    },
    /**
     * Kiểm tra dữ liệu hợp lệ
     * Author: Nguyễn Văn Huy (30/03/2023)
     */
    onValidate() {
      this.errorMessage = null;
      if (this.newAccount.Password && this.newAccount.Password.length > 24) {
        this.errorMessage = "Mật khẩu quá dài";
        this.$refs.txtOldPassword.errorMessage = "Mật khẩu quá dài";
      }
      if (
        this.newAccount.NewPassword &&
        this.newAccount.NewPassword.length > 24
      ) {
        this.errorMessage = "Mật khẩu quá dài";
        this.$refs.txtNewPassword.errorMessage = "Mật khẩu quá dài";
      }
      if (
        this.newAccount.PasswordConfirm &&
        this.newAccount.PasswordConfirm.length > 24
      ) {
        this.errorMessage = "Mật khẩu quá dài";
        this.$refs.txtNewPasswordConfirm.errorMessage = "Mật khẩu quá dài";
      }

      if (this.newAccount.NewPassword != this.newAccount.PasswordConfirm) {
        this.errorMessage = "Xác nhận mật khẩu không khớp";
        this.$refs.txtNewPasswordConfirm.errorMessage =
          "Xác nhận mật khẩu không khớp";
      }

      if (this.newAccount.Password) {
        this.$refs.txtOldPassword.onValidate();
      }
      if (this.newAccount.NewPassword) {
        this.$refs.txtNewPassword.onValidate();
      }
      if (this.newAccount.PasswordConfirm) {
        this.$refs.txtNewPasswordConfirm.onValidate();
      }
    },

    /**
     * Đổi mật khẩu
     * Author: Nguyễn Văn Huy (02/05/2023)
     */
    changePassword() {
      this.onValidate();
      if (!this.errorMessage) {
        var url = `https://localhost:9999/api/v1/Users/ChangePassword/${this.user.UserID}`;
        axios
          .put(url, this.newAccount)
          .then((response) => {
            if (response.data.Success) {
              // Lưu thông tin tài khoản vào localStorage
              this.login(this.newAccount);
            } else {
              this.errorMessage = response.data.UserMsg;
            }
          })
          .catch(() => {
            this.errorMessage = this.HResource.Message.Exception;
          });
      }
    },
  },
};
</script>

<style>
</style>