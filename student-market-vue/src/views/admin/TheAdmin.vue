<template>
  <div class="admin_view">
    <TheSidebar class="av__sidebar" />
    <router-view class="av__content" />
  </div>
</template>

<script>
import TheSidebar from "@/layout/admin/sidebar/TheSidebar.vue";
export default {
  components: { TheSidebar },
  name: "TheAdmin",
};
</script>

<style>
.admin_view {
  flex-wrap: wrap;
  height: 100%;
  background-color: #393a3d;
}
.av__sidebar {
  position: fixed;
  height: calc(100vh - 320px);
  width: 240px;
}
.av__content {
  width: calc(100% - 240px);
  margin-left: 240px;
  background-color: #f4f4f4;
  min-height: calc(100vh - 80px);
}
</style>