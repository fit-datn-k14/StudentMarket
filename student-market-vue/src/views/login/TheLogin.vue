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
        <div class="errorMessage">{{ errorMessage }}</div>
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
    login() {
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
        .catch((error) => {
          console.log(error);
        });
    },
  },
};
</script>

<style scoped>
.form_login {
  margin-top: 48px;
  background-color: #fff;
  padding: 12px;
  border-radius: 8px;
  padding-bottom: 48px;
}

.btnLogin {
  display: flex;
  justify-content: center;
}

.link-register {
  text-align: center;
}

.the-login a {
  text-decoration: none;
}
</style>