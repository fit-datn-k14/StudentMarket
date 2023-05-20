<template>
  <!--  -->
  <div class="input-wrapper">
    <label v-if="label" for="hinput" :title="title"
      >{{ label }}<span v-if="required" class="color-red">&nbsp;*</span></label
    >
    <textarea
      v-if="type == 'textarea'"
      v-model="value"
      :disabled="disabled"
      @input="onInput"
      @blur="onValidate"
    ></textarea>
    <input
      v-else
      ref="hinput"
      :type="type"
      autocomplete="off"
      :class="{ 'error-input': isEmpty }"
      :placeholder="placeholder"
      :disabled="disabled"
      v-model="value"
      @input="onInput"
      @blur="onValidate"
    />
    <div v-show="isEmpty" :class="{ 'error-message': isEmpty }">
      {{ errorMessage }}
    </div>
  </div>
</template>

<script>
export default {
  name: "HInput",
  props: {
    modelValue: [String, Number, Boolean],
    label: {
      type: String,
      required: false,
      default: null,
    },
    type: {
      type: String,
      required: false,
      default: "text",
    },
    required: {
      type: Boolean,
      required: false,
      default: false,
    },
    disabled: {
      type: Boolean,
      required: false,
      default: false,
    },
    title: {
      type: String,
      required: false,
      default: null,
    },
    placeholder: {
      type: String,
      required: false,
      default: null,
    },
  },
  watch: {
    modelValue: function (newVal) {
      this.value = newVal;
    },
  },
  created() {
    if (this.modelValue) {
      this.value = this.modelValue;
    }
  },
  methods: {
    /**
     * binding 2 chiều
     * Author: NVHUY (09/03/2001)
     */
    onInput() {
      if (this.value && this.type != "Number") {
        this.$emit("update:modelValue", this.value.trim());
      } else if (this.value && this.type == "Number") {
        this.$emit("update:modelValue", this.value);
      } else {
        this.$emit("update:modelValue", null);
      }
    },
    /**
     * Kiểm tra dữ liệu trống
     * Author: NVHUY (09/03/2001)
     */
    onValidate() {
      if (this.required && (!this.value || !this.value.trim())) {
        this.isEmpty = true;
        this.errorMessage = this.label + " không được bỏ trống!";
      } else {
        this.isEmpty = false;
        this.errorMessage = "";
      }
    },
  },
  data() {
    return {
      value: "",
      errorMessage: "",
      isEmpty: false,
    };
  },
};
</script>

<style scoped>
</style>

<style scoped>
@import url(@/css/base/input.css);
</style>