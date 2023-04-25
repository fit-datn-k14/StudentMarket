<template>
  <table>
    <thead>
      <tr>
        <th v-if="hasCheckbox" class="checkbox-col">
          <h-checkbox v-model="allSelected" @change="onCheckAll" />
        </th>
        <th
          v-for="(col, index) in columns"
          :key="index"
          :style="'min-width: ' + (col.width ? col.width : columnWidthDefault)"
          :class="HCommon.textAlign(col.dataAlign)"
          :title="col.note"
          class="data-col"
        >
          <span class="btn-filter">
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
                sortConditionInside &&
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
        class="row"
        v-for="(item, indexItem) in dataArray"
        :key="indexItem"
        v-show="displayRow(indexItem)"
        @dblclick="onDblclickRow(item)"
        :class="{
          'selected-row': selectedItems[indexItem],
          'parent-row':
            dataArray[indexItem + 1] &&
            dataArray[indexItem + 1].generation == item.generation + 1,
        }"
      >
        <td v-if="hasCheckbox" class="checkbox-col">
          <h-checkbox
            v-model="selectedItems[indexItem]"
            @change="onClickCheckbox(indexItem)"
          />
        </td>
        <td :style="paddingLeft(item.generation)" class="code-column data-col">
          <div>
            <div class="icon-expand">
              <h-icon
                v-if="
                  dataArray[indexItem + 1] &&
                  dataArray[indexItem + 1].generation == item.generation + 1 &&
                  !item.Expand
                "
                position="-608px -312px"
                size="15 15"
                @click="expand(indexItem)"
              />
              <h-icon
                v-if="
                  dataArray[indexItem + 1] &&
                  dataArray[indexItem + 1].generation == item.generation + 1 &&
                  item.Expand
                "
                position="-560px -312px"
                size="15 15"
                @click="colllapse(indexItem)"
              />
            </div>
            {{ item[firstDataField] }}
          </div>
        </td>
        <td
          v-for="(col, indexCol) in columns.slice(1)"
          :key="indexCol"
          :class="HCommon.textAlign(col.dataAlign)"
          :style="'max-width: ' + (col.width ? col.width : columnWidthDefault)"
          class="data-col"
        >
          {{ HCommon.formatData(col, item[col.dataField]) }}
        </td>
        <td
          v-if="hasWiget"
          class="function-col"
          :class="{ action: indexItem == selectAction }"
        >
          <input
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
                  v-show="displayAction(item, actionItem)"
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
  name: "HGridTree",
  components: {
    HFilterBox,
  },
  props: {
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
  },
  watch: {
    data: function (newVal) {
      this.updateDataArray(newVal);
    },
  },
  created() {
    this.columnWidthDefault = this.HConfig.ColumnWidthDefault;
    this.updateDataArray(this.data);
    this.firstDataField = this.columns[0].dataField;
  },

  computed: {
    paddingLeft() {
      return function (generation) {
        let padding = 24 * generation + 32;
        return `padding-left: ${padding}px`;
      };
    },
  },
  methods: {
    /**
     * Cập nhật dữ liệu cho dataArray
     * Author: NVHuy(19/04/2023)
     */
    updateDataArray(data) {
      this.dataArray = data;
      if (data) {
        this.dataArray.forEach((el) => {
          el.Expand = false;
        });
      }
    },

    displayRow(index) {
      let currentItem = this.dataArray[index];
      if (currentItem && currentItem.generation == 0) {
        return true;
      }
      let parentIndex = index;
      let parentItem = this.dataArray[parentIndex];
      /* eslint-disable */
      while (parentIndex >= 0) {
        parentIndex--;
        parentItem = this.dataArray[parentIndex];
        if (parentItem && parentItem.generation < currentItem.generation) break;
      }
      if (this.displayRow(parentIndex) && parentItem.Expand) {
        return true;
      }
      return false;
    },

    /**
     * Hiện thị các item con
     * Author: NVHuy(19/04/2023)
     */
    expand(index) {
      var item = this.dataArray[index];
      item.Expand = true;
    },

    /**
     * Ẩn các item con
     * Author: NVHuy(19/04/2023)
     */
    colllapse(index) {
      var item = this.dataArray[index];
      item.Expand = false;
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
     * Chọn tất cả và bỏ chọn tất cả
     * Author: Nguyễn Văn Huy (16/03/2023)
     */
    onCheckAll() {
      if (this.allSelected) {
        this.selectedItems = new Array(this.dataArray.length).fill(true);
        this.dataArray.forEach((element) => {
          if (!this.selectedIds.includes(element.EmployeeId)) {
            this.selectedIds.push(element.EmployeeId);
          }
        });
      } else {
        this.selectedItems = new Array(this.dataArray.length).fill(false);
        this.dataArray.forEach((element) => {
          this.removeIdSelected(element.EmployeeId);
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
        this.selectedIds.push(this.dataArray[index].EmployeeId);
      } else {
        this.removeIdSelected(this.dataArray[index].EmployeeId);
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
     * Lọc dữ liệu
     * Author: Nguyễn Văn Huy(02/04/2023)
     */
    onClickFilter() {
      this.$emit("tableFunction", "refresh");
    },

    /**
     * Hiển thị chức năng
     * Author: Nguyễn Văn Huy(02/04/2023)
     */
    displayAction(item, action) {
      if (
        (action.key == "active" && !item.active_status) ||
        (action.key == "inactive" &&
          (item.active_status == 1 || item.active_status == null))
      ) {
        return false;
      } else {
        return true;
      }
    },
  },

  data() {
    return {
      selectColumnFilter: {},
      positionFilter: {},
      selectAction: -1,
      conditions: [],
      allSelected: false,
      selectedItems: [],
      selectedIds: [],
      firstDataField: "",
      columnWidthDefault: "",
      dataArray: [],
    };
  },
};
</script>

<style>
.code-column > div {
  position: relative;
  height: 24px;
  line-height: 24px;
}
.icon-expand {
  position: absolute;
  right: 100%;
  top: 0px;
}
.parent-row {
  font-weight: bold;
}
td.data-col {
  white-space: nowrap; /* không xuống dòng */
  overflow: hidden; /* giới hạn chiều ngang */
  text-overflow: ellipsis; /* thêm dấu "..." */
}
.table {
  width: 100%;
  overflow: auto;
  height: 100px;
}
@import url(@/css/base/table.css);
</style>