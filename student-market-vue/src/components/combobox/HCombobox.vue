<template>
  <div class="input-wrapper">
    <label v-if="label" for="txtFullName" :title="title"
      >{{ label }}<span v-if="required" class="color-red">&nbsp;*</span></label
    >
    <div
      class="hcombobox"
      v-click-outside="closeDataList"
      :class="{ 'error-input': errorMessage }"
    >
      <input
        ref="cbbInput"
        class="hcombobox__input"
        type="text"
        v-model="textInput"
        @focus="showData"
        @input="comboboxSearch"
        @keydown="inputOnKeydown"
        @blur="onValidate"
      />
      <span v-show="errorMessage" :class="{ 'error-message': errorMessage }">
        {{ errorMessage }}
      </span>
      <button class="hcombobox__button" @click="onClickButton" tabindex="-1">
        <div
          class="hcombobox__icon"
          :class="{ 'rotate-180': isShowData && type == 'bottomup' }"
        ></div>
      </button>
      <div v-show="isShowData" class="hcombobox__datalist" :class="type">
        <div class="hcombobox__list">
          <div
            v-if="defaultItem"
            class="hcombobox__option"
            @click="onSelectDataItem(defaultItem)"
            :class="{ 'hcombobox__option--selected': indexSelected == -1 }"
          >
            {{ defaultItem[propText] }}
          </div>
          <div
            v-for="(item, index) in dataFilter"
            :key="index"
            class="hcombobox__option"
            @click="onSelectDataItem(item)"
            :class="{ 'hcombobox__option--selected': index == indexSelected }"
          >
            {{ propText ? item[propText] : item }}
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "HCombobox",
  props: {
    modelValue: {
      type: [String, Boolean, Number, Array],
      required: false,
      default: null,
    },
    label: {
      type: String,
      required: false,
      default: null,
    },
    defaultItem: {
      type: Object,
      required: false,
      default: null,
    },
    required: {
      type: Boolean,
      required: false,
      default: false,
    },
    validate: {
      type: Boolean,
      required: false,
      default: false,
    },
    api: {
      type: String,
      required: false,
      default: null,
    },
    data: {
      type: Array,
      required: false,
    },
    propText: {
      type: String,
      required: false,
      default: null,
    },
    propValue: {
      type: String,
      required: false,
      default: null,
    },
    type: {
      type: String,
      required: false,
      default: "topdown",
    },
  },
  watch: {
    modelValue: function (newVal) {
      if (newVal != null) {
        this.dataArray.forEach((element) => {
          if (element[this.propValue] == newVal) {
            this.textInput = element[this.propText];
            this.indexSelected = this.dataArray.indexOf(element);
          }
        });
      } else {
        if (!this.defaultItem) {
          this.textInput = null;
          this.indexSelected = 0;
        }
      }
    },
  },
  async created() {
    if (this.defaultItem) {
      this.textInput = this.defaultItem[this.propText];
    }
    if (this.api) {
      await fetch(this.api)
        .then((res) => res.json())
        .then((data) => {
          this.dataArray = data.Data;
          this.dataFilter = data.Data;
        });
    } else if (this.data) {
      this.dataArray = this.data;
      this.dataFilter = this.data;
    }
    if (this.modelValue != null) {
      this.dataArray.forEach((element) => {
        if (element[this.propValue] == this.modelValue) {
          this.textInput = element[this.propText];
          this.indexSelected = this.dataArray.indexOf(element);
        }
      });
    }
  },
  methods: {
    showData() {
      this.isShowData = true;
      this.dataFilter = this.dataArray;
      if (this.modelValue != null) {
        this.dataArray.forEach((element) => {
          if (element[this.propValue] == this.modelValue) {
            this.textInput = element[this.propText];
            this.indexSelected = this.dataArray.indexOf(element);
          }
        });
      }
    },
    /**
     * Chức năng search
     * Author: NVHuy(09/03/2023)
     */
    comboboxSearch() {
      this.dataFilter = this.dataArray.filter((item) =>
        item[this.propText].toLowerCase().includes(this.textInput.toLowerCase())
      );
    },
    /**
     * Kiểm tra dữ liệu hợp lệ
     * author: NVHuy (31/03/2023)
     */
    onValidate() {
      if (this.validate) {
        if (!this.$refs.cbbInput.value || !this.$refs.cbbInput.value.trim()) {
          this.errorMessage = this.label + this.HResource.Message.EmptyInput;
        } else if (this.dataFilter.length == 0) {
          this.errorMessage = this.HResource.Message.DataInvalid(
            `<${this.label}>`
          );
        } else {
          this.errorMessage = null;
        }
      }
    },
    /**
     * Sự kiện click chọn 1 dòng
     * @param {*} item
     * author: NVHuy (09/03/2023)
     */
    async onSelectDataItem(item) {
      this.closeDataList();
      if (item) {
        // Hiển thị text của item vừa chọn lên input:
        this.textInput = this.propText ? item[this.propText] : item;
        // Binding 2 chiều
        let value = this.propValue ? item[this.propValue] : item;
        if (value != null) {
          await this.$emit("update:modelValue", value);
        } else {
          await this.$emit("update:modelValue", null);
          this.indexSelected = -1;
        }
      } else {
        await this.$emit("update:modelValue", null);
      }
      // Ẩn đi
      await this.onValidate();
    },
    /**
     * Tạo chức năng chọn item bằng bàn phím (lên xuống Enter)
     * author: NVHuy (09/03/2023)
     */
    inputOnKeydown() {
      const keyCode = event.code;
      switch (keyCode) {
        case "ArrowDown":
          // Chọn item đầu tiên hoặc item phía dưới
          if (this.indexSelected == this.dataFilter.length - 1) {
            this.indexSelected = 0;
          } else {
            this.indexSelected++;
          }
          break;
        case "ArrowUp":
          // Chọn item cuối cùng hoặc item phía trên
          if (!this.indexSelected) {
            this.indexSelected = this.dataFilter.length - 1;
          } else {
            this.indexSelected--;
          }
          break;
        case "Tab":
          this.onSelectDataItem(this.dataFilter[this.indexSelected]);
          // Thực hiện chọn item
          break;
        case "Enter":
          // Thực hiện chọn item
          this.onSelectDataItem(this.dataFilter[this.indexSelected]);
          break;
        default:
          break;
      }
    },

    /**
     * Sự kiện click vào button
     * Author: Nguyễn Văn Huy(32/03/2023)
     */
    onClickButton() {
      this.dataFilter = this.dataArray;
      if (!this.isShowData) {
        this.showData();
      } else {
        this.isShowData = !this.isShowData;
      }
    },

    /**
     * Ẩn danh sách lựa chọn
     * Author: Nguyễn Văn Huy(31/03/2023)
     */
    closeDataList() {
      this.isShowData = false;
    },
  },
  data() {
    return {
      errorMessage: "",
      dataArray: [],
      isShowData: false,
      textInput: "",
      dataFilter: [],
      indexSelected: -1,
    };
  },
};
</script>

<style scoped>
@import url(@/css/base/combobox.css);
</style>