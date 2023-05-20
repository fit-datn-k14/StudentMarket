<template>
  <div class="the_sidebar">
    <div class="menu__list">
      <router-link to="/admin/quan-ly-tin-dang">
        <div class="menu__item">
          <h-icon position="-78px -1588px" size="14 18" />
          <p>Quản lý tin đăng</p>
        </div></router-link
      >
      <router-link to="/admin/quan-ly-nguoi-dung"
        ><div class="menu__item">
          <h-icon position="-427px -1591px" size="16 16" />
          <p>Quản lý người dùng</p>
        </div></router-link
      >
    </div>
  </div>
</template>

<script>
export default {
  name: "TheSidebar",
};
</script>

<style>
.the_sidebar {
  display: flex;
  flex-direction: column;
  row-gap: 4px;
  padding: 12px;
}

.menu__item {
  display: flex;
  width: 100%;
  color: #fff;
  font-size: 14px;
  align-items: center;
  height: 44px;
  border-radius: 4px;
  padding-left: 12px;
  margin-top: 4px;
  box-sizing: border-box;
  column-gap: 12px;
}
.menu__item p {
  margin: 0;
}

.menu__list .router-link-active > div {
  background-color: #616161;
}

.menu__item:hover {
  background-color: #757575;
}
</style>