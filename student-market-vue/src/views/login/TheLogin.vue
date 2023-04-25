<template>
  <div class="container">
    <div class="row">
      <div class="col-md-6 offset-md-3 mt-5">
        <div class="card">
          <div class="card-header">Đăng nhập</div>
          <div class="card-body">
            <form>
              <div class="form-group">
                <label for="username">Tên đăng nhập</label>
                <input
                  type="text"
                  class="form-control"
                  id="username"
                  v-model="username"
                />
              </div>
              <div class="form-group">
                <label for="password">Mật khẩu</label>
                <input
                  type="password"
                  class="form-control"
                  id="password"
                  v-model="password"
                />
              </div>
              <button
                type="submit"
                class="btn btn-primary"
                @click.prevent="login"
              >
                Đăng nhập
              </button>
            </form>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import { saveUserToLocalStorage } from "@/stores/localStorage.js";
export default {
  data() {
    return {
      username: "",
      password: "",
    };
  },
  methods: {
    login() {
      axios
        .post("https://localhost:9999/api/v1/Users/Login", {
          username: this.username,
          password: this.password,
        })
        .then((response) => {
          // Lưu thông tin tài khoản vào localStorage
          var user = response.data.Data;
          saveUserToLocalStorage(user);
          // Chuyển hướng đến trang chủ
          this.$router.push("/trang-chu");
        })
        .catch((error) => {
          console.log(error);
        });
    },
  },
};
</script>

<style scoped>
.login-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  height: 100vh;
  background-color: #f5f5f5;
}

h1 {
  margin-bottom: 2rem;
}

form {
  display: flex;
  flex-direction: column;
  align-items: center;
  width: 100%;
  max-width: 400px;
  background-color: white;
  padding: 2rem;
  border-radius: 5px;
  box-shadow: 0px 2px 5px rgba(0, 0, 0, 0.2);
}

.form-group {
  margin-bottom: 1rem;
}

label {
  display: block;
  margin-bottom: 0.5rem;
  font-weight: bold;
}

input {
  padding: 0.5rem;
  border-radius: 5px;
  border: 1px solid #ccc;
  width: 100%;
}

button[type="submit"] {
  margin-top: 2rem;
  padding: 0.5rem 1rem;
  background-color: #4caf50;
  color: white;
  border: none;
  border-radius: 5px;
  font-size: 1rem;
  cursor: pointer;
  transition: background-color 0.2s ease-in-out;
}

button[type="submit"]:hover {
  background-color: #388e3c;
}
</style>