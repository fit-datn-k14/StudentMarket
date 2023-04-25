<template>
  <div class="app">
    <div style="height: 80px"></div>
    <TheHeader class="the-header" />
    <TheContent class="the-content" />
    <TheFooter class="the-footer" />
  </div>
</template>

<script>
import TheContent from "./layout/TheContent.vue";
import TheFooter from "./layout/TheFooter.vue";
import TheHeader from "./layout/TheHeader.vue";
export default {
  name: "App",
  components: {
    TheHeader,
    TheContent,
    TheFooter,
  },
};
</script>

<style>
@import url(@/css/main.css);
@import url(@/assets/vendor/fontawesome-free-6.4.0-web/css/all.min.css);
.app {
  min-height: 100vh;
  background-color: #f4f4f4;
}
.the-header {
  min-height: 80px;
}
.the-content {
  min-height: calc(100vh - 80px);
}
</style>
