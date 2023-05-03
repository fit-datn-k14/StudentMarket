<template>
  <div class="table">
    <table>
      <thead>
        <tr>
          <th v-if="hasCheckbox" class="checkbox-col">
            <h-checkbox v-model="allSelected" @change="onCheckAll" />
          </th>
          <th
            v-for="(col, index) in columns"
            :key="index"
            :style="'min-width: ' + (col.width ? col.width : '120px')"
            :class="textAlign(col.dataAlign)"
            :title="col.note"
            class="data-col"
            @click="toggleSort(index, $event)"
          >
            <span v-if="col.condition.type" class="btn-filter">
              <h-icon
                class="th-filter-icon"
                position="-1683px -560px"
                @click="openFilter($event, index)"
              />
            </span>
            <div class="th-text">
              {{ col.title.toUpperCase() }}
              <h-icon
                class="btn-sort"
                v-if="
                  sortConditionInside.Option != null &&
                  sortConditionInside.Field == this.columns[index].dataField
                "
                :class="{ 'rotate-180': sortConditionInside.Option == 1 }"
                position="-1756px -317px"
                size="8 5"
              />
            </div>
          </th>
          <th v-if="hasWiget" class="function-col">CHỨC NĂNG</th>
        </tr>
      </thead>
      <tbody>
        <tr
          class="tbrow"
          v-for="(item, indexItem) in dataArray"
          :key="indexItem"
          style="height: 48px"
          :class="{ 'selected-row': selectedItems[indexItem] }"
          @dblclick="onDblclickRow(item)"
        >
          <td v-if="hasCheckbox" class="checkbox-col">
            <h-checkbox
              v-model="selectedItems[indexItem]"
              @change="onClickCheckbox(indexItem)"
            />
          </td>
          <td
            v-for="(col, indexCol) in columns"
            :key="indexCol"
            :class="textAlign(col.dataAlign)"
            class="data-col"
          >
            <h-input v-if="modEdit" v-model="item[col.dataField]"></h-input>
            <div v-if="!modEdit">
              {{ HCommon.formatData(col, item[col.dataField]) }}
            </div>
          </td>
          <td
            v-if="hasWiget"
            class="function-col"
            :class="{ action: indexItem == selectAction }"
          >
            <input
              v-if="modDetail"
              @click="displayDetail(item)"
              class="btn-link btn-edit"
              type="button"
              value="Chi Tiết"
            />
            <input
              v-else
              @click="displayEditPopupDetail(item)"
              class="btn-link btn-edit"
              type="button"
              value="Sửa"
            />
            <div>
              <h-icon
                position="-900px -365px"
                size="8 5"
                @click="toggleDrop(indexItem)"
              />
              <div
                v-if="indexItem == selectAction"
                class="dropdownlist"
                v-click-outside="closeDrop"
              >
                <ul>
                  <li
                    v-for="(actionItem, indexAction) in actions"
                    :key="indexAction"
                  >
                    <input
                      class="btn-link"
                      type="button"
                      :value="actionItem.name"
                      @click="
                        selectAction = -1;
                        onClickAction(actionItem.key, item);
                      "
                    />
                  </li>
                </ul>
              </div>
            </div>
          </td>
        </tr>
      </tbody>
    </table>
    <div v-if="pagination" class="paging">
      <div class="paging__left">
        Tổng số:&nbsp;<strong>{{ totalRecords }}</strong
        >&nbsp;bản ghi
      </div>
      <div class="paging__right">
        <div class="combobox paging__combobox">
          <h-combobox
            :data="pagingOptionArray"
            propText="text"
            propValue="value"
            v-model="pageTable.pageSize"
            type="bottomup"
          ></h-combobox>
        </div>
        <h-paging
          :page-count="totalPages"
          :click-handler="pageChange"
          prev-text="&#10096;"
          next-text="&#10097;"
          :container-class="'pagination'"
          v-model="pageTable.pageNumber"
        ></h-paging>
      </div>
    </div>
  </div>
  <h-filter-box
    ref="boxFilter"
    v-model="selectColumnFilter"
    :position="positionFilter"
    @onClickFilter="onClickFilter"
  />
</template>

<script>
import HFilterBox from "@/components/filterbox/HFilterBox.vue";
export default {
  name: "HTable",
  components: {
    HFilterBox,
  },
  props: {
    modDetail: {
      type: Boolean,
    },
    modelValue: {
      type: [String, Boolean, Number, Array],
      required: false,
      default: null,
    },
    /**
     * Danh sách cấu hình cột của table 
     * gồm các thuộc tính 
     * title : 
        width: 
        data-field:
        formatType:  tiền, số , chữ, enum
        enumName: tên enum để gender ra dữ liệu
        dataAlign: căn chỉnh 
        sortable: cho phép sắp xếp hay ko 

     */
    idField: {
      type: String,
      required: true,
    },
    columns: {
      type: Array,
    },
    /**
     * Danh sách action
     */
    actions: {
      type: Array,
    },

    /**
     * Dữ liệu của bảng
     */
    data: {
      type: Array,
    },

    /**
     * Có cột checkbox không
     */
    hasCheckbox: {
      type: Boolean,
      required: false,
      default: false,
    },

    /**
     * Có cột chức năng không
     */
    hasWiget: {
      type: Boolean,
      required: false,
      default: false,
    },

    /**
     * Có phân trang không
     */
    pagination: {
      type: Boolean,
      required: false,
      default: true,
    },

    /**
     * Tổng số trang
     */
    totalPages: {
      type: Number,
      required: false,
      default: 0,
    },

    /**
     * Tổng số bản ghi
     */
    totalRecords: {
      type: Number,
      required: false,
      default: 1,
    },

    /**
     * Điều kiện sắp xếp dữ liệu theo cột
     * Author: Nguyễn Văn Huy(04/04/2023)
     */
    sortCondition: {
      type: Object,
      required: false,
      default: null,
    },

    /**
     * Thông tin phân trang
     * Author: Nguyễn Văn Huy(04/04/2023)
     */
    pageValue: {
      type: Object,
      required: false,
      default: function () {
        return {
          pageNumber: 1,
          pageSize: 20,
        };
      },
    },
  },
  watch: {
    data: function (newVal) {
      if (newVal) {
        this.dataArray = newVal;
        this.selectedItems = new Array(this.dataArray.length).fill(false);
        this.selectedIds = this.modelValue;
        newVal.forEach((element, index) => {
          if (this.selectedIds.includes(element[this.idField])) {
            this.selectedItems[index] = true;
          }
        });
        this.allSelected = false;
      } else {
        this.dataArray = [];
        this.selectedItems = [];
      }
      if (this.selectedItems.every((value) => value === true)) {
        this.allSelected = true;
      } else {
        this.allSelected = false;
      }
    },
    "pageTable.pageSize": function () {
      this.pageTable.pageNumber = 1;
      this.$emit("tableFunction", "refresh");
    },
  },
  created() {
    this.pageTable = this.pageValue;

    this.selectedItems = new Array(this.dataArray.length).fill(false);
    this.columns.forEach((element) => {
      this.conditions.push(element.condition);
    });
    this.sortConditionInside = this.sortCondition;
  },
  computed: {
    quantity() {
      if (this.data) {
        return this.data.length;
      } else {
        return 0;
      }
    },
  },
  methods: {
    /**
     * sự kiện sắp xếp
     * @param {*} index
     * @param {*} event
     * Author:Nguyễn Văn Huy(02/04/2023)
     */
    toggleSort(index, event) {
      if (!event.target.classList.contains("icon")) {
        if (
          this.sortConditionInside.Option == null ||
          this.sortConditionInside.Field != this.columns[index].dataField
        ) {
          this.sortConditionInside.Field = this.columns[index].dataField;
          this.sortConditionInside.Option = 0;
        } else if (this.sortConditionInside.Option == 0) {
          this.sortConditionInside.Option = 1;
        } else {
          this.sortConditionInside.Field = null;
          this.sortConditionInside.Option = null;
        }
        this.$emit("tableFunction", "refresh");
      }
    },

    /**
     * Đóng danh sách lựa chọn
     * Author: NVHuy (10/03/2023)
     */
    closeDrop() {
      this.selectAction = -1;
    },

    /**
     * Ẩn/ hiện danh sách chức năng
     * Author: Nguyễn Văn Huy (05/03/2023)
     */
    toggleDrop(index) {
      if (this.selectAction != index) {
        this.selectAction = index;
      }
    },

    /**
     * Mở Filter
     * Author: Nguyễn Văn Huy (29/03/2023)
     */
    async openFilter(event, index) {
      await (this.selectColumnFilter = this.columns[index]);
      const rect = event.target.getBoundingClientRect();
      this.positionFilter = {
        left: rect.left,
        top: rect.top,
      };
      this.$refs.boxFilter.open(index);
    },

    /**
     * Định dạng dữ liệu
     * Author: Nguyễn Văn Huy (05/03/2023)
     */
    formatData(col, data) {
      switch (col.formatType) {
        case "date":
          return this.HCommon.formatDate(
            data,
            this.HEnum.DateFormat.TypeForTable
          );

        case "money":
          return this.HCommon.formatMoney(data);
        case "gender":
          return this.HCommon.formatGender(data);
        default:
          return data;
      }
    },

    /**
     * Khi bấm đúp vào dòng hiển thị form chi tiết
     * @param {*} item
     */
    onDblclickRow(item) {
      if (
        event.target.tagName.toLowerCase() != "input" &&
        !event.target.classList.contains("icon-container")
      ) {
        this.displayEditPopupDetail(item);
      }
    },

    /**
     * Mở popup
     * Author: Nguyễn Văn Huy (06/03/2023)
     */
    displayEditPopupDetail(item) {
      this.$emit("tableFunction", "openEditPopup", item);
    },

    /**
     * Đến trang chi tiết
     * Author: Nguyễn Văn Huy (06/03/2023)
     */
    displayDetail(item) {
      this.$emit("tableFunction", "displayDetail", item);
    },

    /**
     * Tạo class căn chỉnh dữ liệu
     * @param {*} dataAlign
     * Author: Nguyễn Văn Huy(04/04/2023)
     */
    textAlign(dataAlign) {
      if (dataAlign) {
        return "text-align-" + dataAlign;
      } else return "text-align-left";
    },

    /**
     * Chọn tất cả và bỏ chọn tất cả
     * Author: Nguyễn Văn Huy (16/03/2023)
     */
    onCheckAll() {
      if (this.allSelected) {
        this.selectedItems = new Array(this.dataArray.length).fill(true);
        this.dataArray.forEach((element) => {
          if (!this.selectedIds.includes(element[this.idField])) {
            this.selectedIds.push(element[this.idField]);
          }
        });
      } else {
        this.selectedItems = new Array(this.dataArray.length).fill(false);
        this.dataArray.forEach((element) => {
          this.removeIdSelected(element[this.idField]);
        });
      }
      this.$emit("update:modelValue", this.selectedIds);
    },

    /**
     * Xoá nhân viên khỏi danh sách chọn
     * Author: Nguyễn Văn Huy (16/03/2023)
     */
    removeIdSelected(id) {
      if (this.selectedIds.includes(id)) {
        const employeeIdToRemove = id;
        this.selectedIds = this.selectedIds.filter(
          (employeeId) => employeeId !== employeeIdToRemove
        );
      }
    },

    /**
     * Sự kiện khi chọn từng checkbox
     * Author: Nguyễn Văn Huy (16/03/2023)
     */
    onClickCheckbox(index) {
      if (this.selectedItems[index]) {
        this.selectedIds.push(this.dataArray[index][this.idField]);
      } else {
        this.removeIdSelected(this.dataArray[index][this.idField]);
      }
      if (this.selectedItems.every((value) => value === true)) {
        this.allSelected = true;
      } else {
        this.allSelected = false;
      }
      this.$emit("update:modelValue", this.selectedIds);
    },

    /**
     * SỰ kiện khi click vào action
     * @param {*} key
     * @param {*} data
     */
    onClickAction(key, data) {
      this.$emit("tableFunction", key, data);
    },

    /**
     * Lọc dữ liệu
     * Author: Nguyễn Văn Huy(02/04/2023)
     */
    onClickFilter() {
      this.pageTable.pageNumber = 1;
      this.$emit("tableFunction", "refresh");
    },

    /**
     * Chuyển trang
     * Author: Nguyễn Văn Huy(04/04/2023)
     */
    pageChange() {
      this.$emit("tableFunction", "refresh");
    },
  },
  data() {
    return {
      pageTable: {},
      dataArray: [],
      sortConditionInside: null,
      conditions: [],
      selectAction: -1,
      positionFilter: {},
      selectColumnFilter: {},
      allSelected: false,
      selectedItems: [],
      selectedIds: [],
      pagingOptionArray: [
        { text: "10 bản ghi trên 1 trang", value: 10 },
        { text: "20 bản ghi trên 1 trang", value: 20 },
        { text: "30 bản ghi trên 1 trang", value: 30 },
        { text: "50 bản ghi trên 1 trang", value: 50 },
        { text: "100 bản ghi trên 1 trang", value: 100 },
      ],
    };
  },
};
</script>

<style scoped>
@import url(@/css/base/table.css);
</style>