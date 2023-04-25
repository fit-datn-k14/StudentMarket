<template>
  <div>
    <div class="shadow"></div>
    <div class="h-dialog-wrapper">
      <div v-if="title" class="h-dialog__title">
        {{ title }}
      </div>
      <div class="h-dialog__content">
        <h-icon
          v-if="type == HEnum.DialogType.Error"
          position="-752px -462px"
          size="36 36"
        ></h-icon>
        <h-icon
          v-if="type == HEnum.DialogType.Confirm"
          position="-832px -462px"
          size="36 36"
        ></h-icon>
        <h-icon
          v-if="type == HEnum.DialogType.Warning"
          position="-598px -463px"
          size="36 37"
        ></h-icon>
        <div v-html="content"></div>
      </div>
      <div class="h-dialog__footer">
        <div>
          <HButton
            ref="cancelButton"
            v-if="type == HEnum.DialogType.Confirm"
            type="btn-second"
            value="Huỷ"
            @click="onClickButton('cancel')"
          ></HButton>
        </div>
        <div>
          <HButton
            ref="closeButton"
            v-if="type == HEnum.DialogType.Error"
            type="btn-pri"
            value="Đóng"
            @click="onClickButton('close')"
          ></HButton>
        </div>
        <div>
          <HButton
            ref="noButton"
            v-if="type > HEnum.DialogType.Error"
            type="btn-second"
            value="Không"
            @click="onClickButton('no')"
          ></HButton>
          <HButton
            v-if="type > HEnum.DialogType.Error"
            type="btn-pri"
            value="Có"
            @click="onClickButton('yes')"
            @keydown.tab.prevent="tabButtonYes"
          ></HButton>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "HDialog",
  props: {
    content: {
      type: String,
      required: false,
      default: null,
    },
    type: {
      type: Number,
      required: false,
      default: 0,
    },
    title: {
      type: String,
      required: false,
      default: null,
    },
  },
  mounted() {
    this.focusButtonDefault();
  },
  methods: {
    /**
     * Sự kiện focus vào nút khi mở
     * Author: Nguyễn Văn Huy(04/04/2023)
     */
    focusButtonDefault() {
      if (this.$refs.closeButton) {
        this.$refs.closeButton.$refs.hbutton.focus();
      } else if (this.$refs.cancelButton) {
        this.$refs.cancelButton.$refs.hbutton.focus();
      } else if (this.$refs.noButton) {
        this.$refs.noButton.$refs.hbutton.focus();
      }
    },

    /**
     * Sự kiện khi bấm nút
     * @param {*} key
     * Author: Nguyễn Văn Huy(04/04/2023)
     */
    onClickButton(key) {
      this.$emit("dialogEvent", key);
    },

    /**
     * Sự kiện tab khỏi nút Có
     * @param {*} event
     * Author: Nguyễn Văn Huy(04/04/2023)
     */
    tabButtonYes(event) {
      event.preventDefault();
      this.focusButtonDefault();
    },
  },
};
</script>

<style  scoped>
@import url(@/css/base/dialog.css);
</style>