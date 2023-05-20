<template>
  <div
    ref="toast"
    v-show="isShowToast"
    class="toastct"
    @animationend="onAnimationEnd"
  >
    <div :class="`toast--${type}`">
      <div class="toast__container">
        <div class="toast__icon"></div>
        <div class="toast__title">{{ toastTitle }}</div>
        <div class="toast__body-text">{{ content }}</div>
        <input class="toast__close" type="button" @click="hideToast" />
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "HToast",
  computed: {
    toastTitle() {
      switch (this.type) {
        case "success":
          return "Thành công!";
        case "warning":
          return "Cảnh báo!";
        case "info":
          return "Thông tin!";
        case "error":
          return "Lỗi!";
        default:
          return null;
      }
    },
  },
  methods: {
    hideToast() {
      if (this.$refs.toast) {
        this.$refs.toast.classList.add("hide_toast");
        setTimeout(() => {
          this.$refs.toast.classList.remove("hide_toast");
          this.isShowToast = false;
        }, 300);
      }
    },
    showToast(content, type) {
      if (content) {
        this.content = content;
        if (type) {
          this.type = type;
        } else {
          this.type = "success";
        }
        this.isShowToast = true;
        setTimeout(this.hideToast, 1500);
      }
    },
  },
  data() {
    return {
      isShowToast: false,
      content: null,
      type: "success",
    };
  },
};
</script>

<style scoped>
@import url(@/css/base/toast.css);
</style>  