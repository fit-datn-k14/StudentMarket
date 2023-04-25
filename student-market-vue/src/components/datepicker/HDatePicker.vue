<template>
  <div class="input-wrapper">
    <label v-if="label" for="txtFullName" :title="title"
      >{{ label }}<span v-if="required" class="color-red">&nbsp;*</span></label
    >
    <input
      ref="hdatepicker"
      type="date"
      :class="{ 'error-input': errorMessage, 'datepicker-default': !valueDate }"
      v-model="valueDate"
      @change="onChangeDatePicker"
      min="1000-01-01"
      max="9999-12-31"
    />
    <div class="datepicker-icon">
      <div></div>
    </div>
    <span
      v-show="errorMessage"
      :class="{ 'error-message-date': errorMessage }"
      >{{ errorMessage }}</span
    >
  </div>
</template>
  
<script>
export default {
  props: {
    label: Date,
    modelValue: {
      type: String,
      required: false,
      default: null,
    },
    hasValidate: {
      type: Boolean,
      required: false,
      default: false,
    },
  },
  watch: {
    modelValue: function (newVal) {
      this.valueDate = newVal;
    },
  },
  created() {
    this.valueDate = this.modelValue;
  },
  methods: {
    onChangeDatePicker() {
      if (this.hasValidate) {
        if (new Date(this.$refs.hdatepicker.value) > new Date()) {
          this.errorMessage = this.label + " không thể lớn hơn ngày hiện tại";
        } else {
          this.errorMessage = null;
        }
      }
      this.$emit("update:modelValue", this.$refs.hdatepicker.value);
    },
  },
  data() {
    return {
      valueDate: null,
      errorMessage: "",
    };
  },
};
</script>

<style scoped>
@import url(@/css/base/datepicker.css);
</style>
  