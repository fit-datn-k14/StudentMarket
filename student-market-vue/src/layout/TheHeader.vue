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
              to="/"
              class="favorite-post-link"
              title="Danh sách tin yêu thích"
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
            <router-link to="/dang-nhap" class="link">
              <div class="text">Đăng nhập</div>
              <div class="arrow"></div>
            </router-link>
            <span class="line"></span>
            <router-link to="/dang-ky" class="link">
              <div class="text">Đăng ký</div>
              <div class="arrow"></div>
            </router-link>

            <router-link to="/dang-tin">
              <HButton
                class="circle"
                ref="btnDangTin"
                value="Đăng Tin"
                type="btn-pri"
                :disabled="false"
              />
            </router-link>
          </div>
        </div>
      </div>
    </header>
  </div>
</template>
  
  <script>
import { eventBus } from "@/js/eventbus";
export default {
  name: "TheHeader",
  methods: {
    onSearch() {
      eventBus.emit("search", this.txtSearch);
      this.$router.push("/trang-chu");
    },
  },
  data() {
    return {
      showDropdown: false,
      txtSearch: "",
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
          title: "Quản Lý Tin Đăng",
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
  width: 460px;
}
.searchbox {
  position: relative;
  margin: 0 28px;
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
</style>
  