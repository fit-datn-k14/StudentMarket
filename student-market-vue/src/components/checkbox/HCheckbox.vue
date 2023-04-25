<template>
  <div class="checkbox">
    <input type="checkbox" :checked="isChecked" @change="updateCheck" />
    <span class="checkmark"></span>
  </div>
</template>

<script>
export default {
  // eslint-disable-next-line
  name: "HCheckbox",
  props: {
    modelValue: {
      type: Boolean,
    },
    label: String,
  },
  data() {
    return {
      isChecked: false,
    };
  },
  watch: {
    modelValue: function (newVal) {
      this.isChecked = newVal;
    },
  },
  mounted() {
    this.isChecked = this.modelValue;
  },
  methods: {
    updateCheck() {
      this.isChecked = !this.isChecked;
      this.$emit("update:modelValue", this.isChecked);
    },
  },
};
</script>

<style scoped>
@import url(@/css/base/checkbox.css);
</style>