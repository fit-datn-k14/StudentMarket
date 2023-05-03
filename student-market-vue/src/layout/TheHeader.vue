<template>
  <div class="header-box shadow-sm">
    <header class="w-100">
      <div
        class="w-100 nav-box d-flex justify-content-between align-items-center"
      >
        <div class="menu-with-logo d-flex align-items-center">
          <div class="logo ms-3">
            <router-link class="header__logo" to="/">
              <img src="@/assets/img/Logo.jpg" alt="" class="logo-img" />
              <strong style="white-space: nowrap; font-size: 20px"
                >Chợ Sinh Viên</strong
              >
            </router-link>
          </div>
          <ul class="menu d-flex">
            <li v-for="(tab, indexTab) in headerList" :key="indexTab">
              <router-link :to="tab.link" class="link">
                <div class="text">{{ tab.title }}</div>
                <div class="arrow"></div>
              </router-link>
            </li>
            <li v-if="censor">
              <router-link to="/admin" class="link">
                <div class="text">Admin</div>
                <div class="arrow"></div>
              </router-link>
            </li>
          </ul>
        </div>
        <div class="searchbox">
          <h-input
            ref="txtSearch"
            v-model="txtSearch"
            @keyup.enter="onSearch"
            placeholder="Tìm kiếm"
          />
          <div class="icon-search" @click="onSearch">
            <h-icon position="-956px -31px" size="22 22" />
          </div>
        </div>
        <div class="login-group me-4 d-flex align-items-center">
          <div class="all-btn d-flex align-items-center">
            <router-link
              to="/tin-nhan"
              class="favorite-post-link"
              title="Tin nhắn"
            >
              <i class="header-icon fa-sharp fa-solid fa-comment"></i>
            </router-link>
            <router-link
              to="/"
              class="favorite-post-link"
              title="Danh sách tin yêu thích"
            >
              <i class="header-icon d-block fa-regular fa-heart"></i>
            </router-link>

            <div><i class="bell-icon fa-solid fa-bell header-icon"></i></div>

            <router-link to="/dang-tin">
              <HButton
                class="circle"
                ref="btnDangTin"
                value="Đăng Tin"
                type="btn-pri"
                :disabled="false"
              />
            </router-link>
            <router-link v-if="!user" to="/dang-nhap" class="link">
              <div class="text">Đăng nhập</div>
              <div class="arrow"></div>
            </router-link>
            <span v-if="!user" class="line"></span>
            <router-link v-if="!user" to="/dang-ky" class="link">
              <div class="text">Đăng ký</div>
              <div class="arrow"></div>
            </router-link>
            <div
              v-else
              class="header__userinfo"
              @click="showDropdown = !showDropdown"
            >
              <img
                class="theheader__avatar"
                :src="URL + `Images/users/avatar/${user.Avatar}`"
              />
              <span>{{ user.FullName }}</span>
              <div v-show="showDropdown" class="header__dropdown">
                <router-link to="/cai-dat-tai-khoan" class="link">
                  <div class="text">Cài Đặt Tài Khoản</div>
                </router-link>
                <router-link to="/doi-mat-khau" class="link">
                  <div class="text">Đổi mật khẩu</div>
                </router-link>
                <HButton type="btn-link" value="Đăng Xuất" @click="logout" />
              </div>
            </div>
          </div>
        </div>
      </div>
    </header>
  </div>
</template>
  
  <script>
import { isCensor } from "../stores/utils.js";
import { eventBus } from "@/js/eventbus";
import {
  getUserFromLocalStorage,
  removeUserFromLocalStorage,
} from "@/stores/localStorage.js";
import HConfig from "@/js/base/config.js";
export default {
  name: "TheHeader",
  created() {
    this.setUser();
  },
  methods: {
    onSearch() {
      eventBus.emit("search", this.txtSearch);
      this.$router.push("/trang-chu");
    },
    logout() {
      removeUserFromLocalStorage();
      this.setUser();
    },
    setUser() {
      this.user = getUserFromLocalStorage();
      this.censor = isCensor();
    },
  },
  mounted() {
    eventBus.on("login", () => {
      this.setUser();
    });
  },
  data() {
    return {
      URL: HConfig.URL,
      showDropdown: false,
      txtSearch: "",
      censor: false,
      user: {},
      userInfo: {},
      headerList: [
        {
          link: "/trang-chu",
          icon: "",
          title: "Trang Chủ",
        },
        {
          link: "/tin-dang",
          icon: "",
          title: "Tin Đăng",
        },
        {
          link: "/quan-ly-tin-dang",
          icon: "",
          title: "Tin Của Tôi",
        },
      ],
    };
  },
};
</script>
  
<style >
div.text {
  white-space: nowrap;
}
.header__logo strong {
  font-size: 24px;
  color: var(--primary-color);
}
.header-box {
  position: fixed;
  top: 0;
  right: 0;
  left: 0;
  background-color: #fff;
  z-index: 999;
}
.logo {
  width: 184px;
}
.logo-img {
  height: 60px;
  width: 60px;
}

.menu {
  margin-bottom: 0;
}

.menu li {
  margin-left: 24px;
  padding: 8px;
  height: 100%;
}

.menu li a {
  color: var(--text-color);
  font-weight: bold;
  line-height: 54px;
  cursor: pointer;
}

.header-box a {
  text-decoration: none;
}

.header-box li:hover a {
  color: var(--primary-color);
}

.arrow {
  height: 2px;
  width: 0%;
  background: #e03c31;
  line-height: 0px;
  font-size: 0px !important;
  margin-top: 2px;
  transition: 0.3s;
}

.menu li:hover .arrow {
  width: 100%;
}

.header-icon {
  font-size: 24px;
  cursor: pointer;
}

.header-icon:hover {
  color: var(--primary-color);
}

.login-group a {
  color: var(--text-color);
  padding: 8px 11px;
  display: block;
  font-weight: bold;
  border: 1px solid transparent;
}

.login-group a:hover {
  background: #fafafa;
  border: solid 1px #fafafa;
  color: var(--primary-color);
}

.btn-post {
  border: 1px solid #ccc;
  border-radius: 8px;
  text-decoration: none;
}

.btn-post:hover {
  border: 1px solid #ccc !important;
  background: #fafafa;
}

.line {
  border: 0;
  width: 1px;
  height: 16px;
  background: #e5e5e5;
  margin: 15px 0;
}
.header-box .searchbox {
  margin: 0 28px;
  width: 460px;
}
.searchbox {
  position: relative;
  max-width: 720px;
  flex-grow: 1;
}

.searchbox > .icon-search {
  position: absolute;
  right: 6px;
  top: 6px;
}

.circle input {
  border-radius: 18px;
}

div:has(.bell-icon):hover bell-icon {
  color: var(--primary-color);
}
.bell-icon {
  padding: 8px;
}

.header__userinfo {
  display: flex;
  column-gap: 8px;
  align-items: center;
  border: 1px solid var(--border-color);
  padding: 2px;
  border-radius: 20px;
  position: relative;
  cursor: pointer;
}

.header__userinfo img {
  width: 36px;
  height: 36px;
  border-radius: 18px;
}

.header__userinfo span {
  white-space: nowrap;
  overflow: hidden;
  font-size: 16px;
  padding-right: 8px;
}

.header__dropdown {
  position: absolute;
  width: 100%;
  border-radius: 4px;
  right: 0;
  top: 42px;
  background-color: #fff;
  border: 1px solid var(--border-color);
}

.header__dropdown input {
  font-weight: bold;
  text-decoration: unset;
  padding: 8px 11px;
}

.header__dropdown input:hover {
}

.header__dropdown > div:hover input,
.header__dropdown > a:hover {
  background-color: #fff;
  color: var(--primary-color);
}
</style>
  