<template>
  <div class="register">
    <div class="row justify-content-center">
      <div class="col-md-6 row g-lg-3">
        <h2>Đăng Ký</h2>
        <h-input
          ref="txtUserName"
          label="Tài khoản"
          :required="true"
          v-model="newAccount.UserName"
        />
        <h-input
          ref="txtFullName"
          label="Họ tên"
          :required="true"
          :regexString="regexFullName"
          v-model="newAccount.FullName"
        />
        <h-input
          ref="txtPassword"
          label="Mật khẩu"
          type="password"
          :required="true"
          v-model="newAccount.Password"
        />
        <h-input
          ref="txtPasswordConfirm"
          label="Nhập lại mật khẩu"
          type="password"
          :required="true"
          v-model="newAccount.PasswordConfirm"
        />
        <div class="errorMessage text-danger">{{ errorMessage }}</div>
        <HButton
          class="btnRegister"
          type="btn-pri"
          ref="btnRegister"
          value="Đăng ký"
          title="Đăng ký"
          @click="register"
        ></HButton>
        <div class="link-login">
          Bạn đã có tài khoản?
          <router-link to="/dang-nhap">Đăng nhập ngay</router-link>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import { eventBus } from "@/js/eventbus";
import axios from "axios";
import { saveUserToLocalStorage } from "@/stores/localStorage";
export default {
  name: "TheRegister",
  data() {
    return {
      newAccount: {},
      errorMessage: "",
      regexFullName: "/^[a-zA-Z _-]+$/",
    };
  },
  methods: {
    login(user) {
      axios
        .post("https://localhost:9999/api/v1/Users/Login", {
          UserName: user.UserName,
          Password: user.Password,
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
      this.$refs.txtUserName.onValidate();
      this.$refs.txtFullName.onValidate();
      this.$refs.txtPassword.onValidate();
      this.$refs.txtPasswordConfirm.onValidate();
      if (!this.newAccount.Password) {
        this.errorMessage = "Mật khẩu không được bỏ trống";
      }
      if (this.newAccount.Password != this.newAccount.PasswordConfirm) {
        this.errorMessage = "Mật khẩu không trùng khớp";
        this.$refs.txtPasswordConfirm.errorMessage =
          "Mật khẩu không trùng khớp";
      }
      if (!this.newAccount.FullName) {
        this.errorMessage = "Tên không được bỏ trống";
      }
      if (this.newAccount.UserName && this.newAccount.UserName.length > 24) {
        this.errorMessage = "Tài khoản quá dài";
        this.$refs.txtUserName.errorMessage = "Tài khoản quá dài";
      }
      if (!this.newAccount.UserName) {
        this.errorMessage = "Tài khoản không được bỏ trống";
      }
    },

    register() {
      this.onValidate();
      if (!this.errorMessage) {
        axios
          .post("https://localhost:9999/api/v1/Users/Register", this.newAccount)
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

<style scoped>
@import url(@/css/views/register.css);
</style>