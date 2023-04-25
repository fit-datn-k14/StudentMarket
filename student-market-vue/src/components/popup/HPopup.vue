<template>
  <div>
    <div class="shadow"></div>
    <div class="popup">
      <div v-if="hasHelp" class="icon popup__btn-help" title="Hỗ trợ"></div>
      <div
        id="btnClose"
        class="icon popup__btn-close"
        title="Đóng"
        @click="closePopup"
      ></div>
      <div v-if="title" class="popup__title">
        <div class="popup__title-text">{{ this.title }}</div>
      </div>
      <div class="popup__content">
        <slot></slot>
      </div>
      <div class="popup__footer">
        <div>
          <HButton type="btn-second" id="btnStore" value="Cất" />
          <HButton
            type="btn-pri"
            id="btnStoreAndAdd"
            value="Cất và Thêm"
            @click="focusFirstInput"
          ></HButton>
        </div>
        <div>
          <HButton
            type="btn-second"
            @click="closePopup"
            ref="btnCancel"
            value="Hủy"
            @blur="focusFirstInput"
            tabindex="0"
          />
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "HPopup",
  props: {
    modelValue: [String, Number, Boolean],
    title: {
      type: String,
      required: false,
    },
  },
  created() {},
  methods: {
    /**
     * Đóng popup
     * Author: huynv (02/03/2023)
     */
    closePopup() {
      this.$emit("close");
    },
  },
  data() {
    return {};
  },
};
</script>

<style>
</style>