<template>
  <div>
    <div class="hbutton-wrapper">
      <input
        ref="hbutton"
        type="button"
        :class="type"
        :id="id"
        :value="value"
        :disabled="disabled"
      />
      <div v-if="title" class="tooltip" v-html="title"></div>
    </div>
  </div>
</template>

<script>
/**
 * Button Component
 * @prop {string} type - (btn-pri, btn-second, btn-link, btn-icon)
 * @prop {string} id - id of input
 * @prop {string} value [isDisabled=false]- value of input
 * Author: Nguyễn Văn Huy (02/03/2023)
 */
export default {
  /* eslint-disable */
  name: "HButton",
  props: {
    id: {
      type: String,
      required: false,
    },
    value: {
      type: String,
      required: false,
    },
    title: {
      type: String,
      required: false,
      default: "",
    },
    type: {
      type: String,
      required: true,
    },
    disabled: {
      type: Boolean,
      required: false,
      default: false,
    },
  },
  // ...
  data() {
    // return {
    //   classList: this.disabled ? this.type + " disabled" : this.type,
    // };
  },
};
</script>

<style scoped>
@import url(@/css/base/button.css);
</style>