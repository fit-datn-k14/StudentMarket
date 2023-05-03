<template>
  <div
    v-if="isShowFilter"
    class="filterBox"
    v-click-outside="close"
    :style="`left:${position.left - 296}px; top:${position.top + 36}px;`"
  >
    <div class="filterBox__title">
      <span>Lọc&nbsp;{{ titleFilter }}</span>
      <div v-if="optionsOperator">
        <div class="btn-operator" @click="toggleOperatorList">
          {{ HResource.operatorText(this.operatorValue) }}
          <h-icon position="-178px -364px" size="12 8" />
        </div>
        <div
          v-if="isShowOptionsOperator"
          class="listOptionsOperator"
          v-click-outside="toggleOperatorList"
        >
          <div v-for="(option, index) in optionsOperator" :key="index">
            <input
              class="operatorOption"
              type="button"
              :value="HResource.operatorText(option)"
              @click="onClickOptionOperator(index)"
            />
          </div>
        </div>
      </div>
    </div>
    <h-input
      ref="txtFilterText"
      v-if="conditionType == 'contains'"
      v-model="txtFilter"
      @keyup.enter="onClickFilter"
    />
    <h-date-picker v-if="conditionType == 'date'" v-model="txtFilter" />
    <h-combobox
      v-if="conditionType == 'select'"
      :data="dataCombobox"
      propText="text"
      propValue="value"
      v-model="txtFilter"
    />
    <div class="filterBox__footer">
      <h-button type="btn-second" value="Bỏ lọc" @click="onClickUnfiltered">
      </h-button>
      <h-button type="btn-pri" value="Lọc" @click="onClickFilter"></h-button>
    </div>
  </div>
</template>

<script>
export default {
  name: "HFilterBox",
  props: {
    modelValue: Object,
    position: {
      type: Object,
      required: true,
    },
  },
  created() {
    this.column = this.modelValue;
  },
  methods: {
    /**
     * Đóng hộp lọc
     * Author: Nguyễn Văn Huy(02/04/2023)
     */
    close() {
      if (this.allowCloseFilterBox) {
        this.isShowFilter = false;
        this.selectedColumn = -1;
      }
      this.allowCloseFilterBox = true;
    },
    /**
     * Mở hộp lọc
     * Author: Nguyễn Văn Huy(02/04/2023)
     */
    open(index) {
      if (index != this.selectedColumn && this.selectedColumn != -1) {
        this.allowCloseFilterBox = false;
      }
      this.selectedColumn = index;
      this.isShowFilter = true;
      this.column = this.modelValue;
      this.titleFilter = this.modelValue["title"];
      this.formatType = this.modelValue["formatType"];
      this.buildDataCombobox();
      this.conditionType = this.modelValue.condition.type;
      this.operatorValue = this.modelValue.condition.operator;
      this.txtFilter = this.modelValue.condition.conditionValue;
    },
    buildDataCombobox() {
      switch (this.formatType) {
        case "gender":
          this.dataCombobox = this.dataComboboxGender;
          break;
        case "approved":
          this.dataCombobox = this.dataComboboxApproved;
          break;
        default:
          this.dataCombobox = this.dataComboboxGender;
      }
      console.log(this.dataCombobox);
    },
    /**
     * Thực hiện lọc
     * Author: Nguyễn Văn Huy(02/04/2023)
     */
    onClickFilter() {
      if (this.column.condition.type == "contains") {
        this.txtFilter = this.$refs.txtFilterText.$refs.hinput.value;
      }
      if (
        this.column.condition.conditionValue != this.txtFilter ||
        this.column.condition.operator != this.operatorValue
      ) {
        this.column.condition.operator = this.operatorValue;
        if (
          this.operatorValue == "isNull" ||
          this.operatorValue == "isNotNull"
        ) {
          this.column.condition.conditionValue = null;
        } else {
          this.column.condition.conditionValue = this.txtFilter;
        }
        this.isShowFilter = false;
        this.$emit("onClickFilter");
      } else {
        this.isShowFilter = false;
      }
    },
    /**
     * Bỏ lọc cột
     * Author: Nguyễn Văn Huy(02/04/2023)
     */
    onClickUnfiltered() {
      if (this.column.condition.conditionValue != null) {
        this.column.condition.conditionValue = null;
        if (this.optionsOperator) {
          this.column.condition.operator = this.optionsOperator[0];
        }
        this.isShowFilter = false;
        this.$emit("onClickFilter");
      } else {
        this.isShowFilter = false;
      }
    },
    /**
     * Ẩn/ Hiện danh sách phép toán lọc
     * Author: Nguyễn Văn Huy(02/04/2023)
     */
    toggleOperatorList() {
      this.isShowOptionsOperator = !this.isShowOptionsOperator;
    },
    /**
     * Chọn phép toán lọc
     * @param {*} index
     * Author: Nguyễn Văn Huy(02/04/2023)
     */
    onClickOptionOperator(index) {
      this.operatorValue = this.optionsOperator[index];
      this.toggleOperatorList();
    },
  },
  computed: {
    optionsOperator() {
      if (this.conditionType == "contains") {
        return ["contains", "startWith", "endWith", "isNull", "isNotNull"];
      }
      if (this.conditionType == "date") {
        return [
          "smaller",
          "smallerOrEqual",
          "bigger",
          "biggerOrEqual",
          "isNull",
          "isNotNull",
        ];
      }
      return null;
    },
  },
  data() {
    return {
      dataCombobox: [],
      dataComboboxGender: [
        { text: "Nữ", value: "0" },
        { text: "Nam", value: "1" },
        { text: "Khác", value: "2" },
      ],
      dataComboboxApproved: [
        { text: "Chưa duyệt", value: "0" },
        { text: "Đã duyệt", value: "1" },
        { text: "Từ chối", value: "2" },
      ],
      allowCloseFilterBox: true,
      isShowOptionsOperator: false,
      titleFilter: "",
      formatType: "",
      operatorValue: "",
      selectedColumn: -1,
      conditionType: "",
      column: {},
      txtFilter: null,
      isShowFilter: false,
    };
  },
};
</script>

<style>
@import url(@/css/base/filterbox.css);
</style>