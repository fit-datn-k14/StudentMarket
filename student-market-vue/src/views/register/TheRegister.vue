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
          :required="true"
          v-model="newAccount.Password"
        />
        <h-input
          ref="txtPasswordConfirm"
          label="Nhập lại mật khẩu"
          :required="true"
          v-model="newAccount.PasswordConfirm"
        />
        <div class="errorMessage">{{ errorMessage }}</div>
        <HButton
          class="btnRegister"
          type="btn-pri"
          ref="btnRegister"
          value="Đăng ký"
          title="Đăng ký"
          @click="register"
        ></HButton>
      </div>
    </div>
  </div>
</template>
<script>
import axios from "axios";
import { saveUserToLocalStorage } from "@/stores/localStorage";
export default {
  data() {
    return {
      newAccount: {},
      errorMessage: "",
      regexFullName: "/^[a-zA-Z _-]+$/",
    };
  },
  methods: {
    /**
     * Kiểm tra dữ liệu hợp lệ
     * Author: Nguyễn Văn Huy (30/03/2023)
     */
    onValidate() {
      if (this.newAccount.UserName && this.newAccount.UserName.length > 24) {
        this.errorMessage = "Tài khoản quá dài";
        console.log(this.errorMessage);
        this.$refs.txtUserName.errorMessage = "Tài khoản quá dài";
        console.log(this.$refs.txtUserName.errorMessage);
      }

      if (this.newAccount.FullName) {
        this.$refs.txtFullName.onValidateFieldEmpty();
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
              saveUserToLocalStorage("user", response.data);
              // Chuyển hướng đến trang chủ
              this.$router.push("/trang-chu");
            }
          })
          .catch((error) => {
            console.log(error);
          });
      }
    },
  },
};
</script>

<style>
.btnRegister .btn-pri {
  width: 100%;
}
.btnRegister {
  margin-top: 24px;
}
.register {
  margin: auto;
  width: 900px;
  background-color: #fff;
  padding: 24px;
}
</style>