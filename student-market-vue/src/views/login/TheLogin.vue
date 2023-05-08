<template>
  <div class="the-login">
    <div class="row justify-content-center">
      <div class="form_login col-md-3 row g-lg-3">
        <h2>Đăng Nhập</h2>
        <h-input
          ref="txtUserName"
          label="Tên Đăng Nhập"
          :required="true"
          v-model="username"
        />
        <h-input
          ref="txtPassword"
          label="Mật Khẩu"
          type="password"
          :required="true"
          v-model="password"
        />
        <div class="errorMessage text-danger">{{ errorMessage }}</div>
        <HButton
          class="btnLogin"
          ref="btn-login"
          type="btn-pri"
          value="Đăng Nhập"
          @click.prevent="login"
        />
        <div class="link-register">
          Bạn chưa có tài khoản?
          <router-link to="/dang-ky">Đăng ký ngay</router-link>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import { eventBus } from "@/js/eventbus";
import { saveUserToLocalStorage } from "@/stores/localStorage.js";
export default {
  name: "TheLogin",
  data() {
    return {
      username: "",
      password: "",
      errorMessage: null,
    };
  },
  methods: {
    /**
     * Kiểm tra dữ liệu hợp lệ
     * Author: Nguyễn Văn Huy (30/03/2023)
     */
    onValidate() {
      this.errorMessage = null;
      this.$refs.txtUserName.onValidate();
      this.$refs.txtPassword.onValidate();
      if (!this.password) {
        this.errorMessage = "Mật khẩu không được bỏ trống";
      }
      if (this.username && this.username.length > 24) {
        this.errorMessage = "Tài khoản quá dài";
        this.$refs.txtUserName.errorMessage =
          "Tài khoản có độ dài tối đa 24 ký tự";
      }
      if (!this.username) {
        this.errorMessage = "Tài khoản không được bỏ trống";
      }
    },

    login() {
      this.onValidate();
      axios
        .post("https://localhost:9999/api/v1/Users/Login", {
          UserName: this.username,
          Password: this.password,
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
  },
};
</script>

<style scoped>
@import url(@/css/views/login.css);
</style>