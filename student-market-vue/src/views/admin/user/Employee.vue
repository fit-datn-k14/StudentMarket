<template>
  <div>
    <div class="content__title">
      <b> Nhân viên</b>
      <HButton
        @click="openPopupDetail"
        id="btnopenPopupDetail"
        value="Thêm mới nhân viên"
        type="btn-pri"
        :disabled="false"
      />
    </div>
    <div class="content__body">
      <div class="table__top">
        <div class="table__top-left">
          <div>
            <button
              class="btn-multidelete"
              :disabled="!isShowMultiDelete"
              @click="isShowDropList = true"
            >
              Thực hiện hàng loạt
              <h-icon position="-564px -365px" size="8 5" />
            </button>
            <div
              v-if="isShowDropList"
              class="table-top__dropdownlist"
              v-click-outside="() => (isShowDropList = false)"
              @click="onClickMultiDelete"
            >
              <div>Xoá</div>
            </div>
          </div>
          <span
            v-for="(col, indexCol) in filteredColumns"
            class="filter-item"
            :key="indexCol"
            >{{ this.HResource.formatFilterText(col) }}
            <h-icon
              position="-84px -316px"
              size="8 8"
              @click="unFilter(col.Index)"
            />
          </span>
          <span
            v-if="filteredColumns.length > 0"
            class="filter-item"
            @click="unFilter()"
          >
            Xoá điều kiện lọc</span
          >
        </div>
        <div class="table__top-right">
          <div class="searchbox">
            <input
              ref="txtSearch"
              type="text"
              :placeholder="HResource.Text.Search"
              autocomplete="off"
              style="height: 32px"
              @change="search"
              @keyup.enter="search"
            />
            <h-icon position="-956px -31px" size="22 22" title="Tìm kiếm" />
          </div>
          <h-icon
            position="-425px -201px -1098px -89px"
            size="21 23 20 22"
            title="Lấy lại dữ liệu"
            @click="loadData"
          />
          <h-icon
            position="-705px -202px -705px -258px"
            size="23 20"
            title="Xuất ra Excel"
            @click="exportExcel"
          />
        </div>
      </div>
      <HTable
        class="content__table"
        v-model="selectedListIds"
        :modEdit="true"
        idField="EmployeeId"
        :columns="columns"
        :data="employees"
        :hasCheckbox="true"
        :hasWiget="true"
        :actions="tableActions"
        :pageValue="pageValue"
        :sortCondition="sortCondition"
        :totalRecords="totalRecords"
        :totalPages="totalPages"
        @tableFunction="eventTable"
      />
    </div>
    <EmployeeDetail
      v-if="isShowPopup"
      :title="popupTitle"
      @eventDetail="eventDetail"
      :employeeInput="employeeSelected"
    ></EmployeeDetail>
    <h-dialog
      v-if="errorMessage"
      :content="errorMessage"
      :type="dialogType"
      @dialogEvent="dialogEvent"
    />
    <h-toast ref="toast" />
    <h-loading v-if="showLoading"></h-loading>
  </div>
</template>
  
<script>
import EmployeeDetail from "@/views/employee/EmployeeDetail.vue";
import { saveAs } from "file-saver";
export default {
  name: "TheEmployees",
  components: {
    EmployeeDetail,
  },
  created() {
    this.columns = this.HConfig.ListEmployeeColumns;
    this.toast = { content: null, type: null };
    this.loadData();
    window.addEventListener("keydown", this.keydownList);
  },
  beforeUnmount() {
    window.removeEventListener("keydown", this.keydownList);
  },

  computed: {
    isShowMultiDelete() {
      return this.selectedListIds.length >= 2;
    },
    filterQuery() {
      var conditions = [];
      this.columns.forEach(function (element) {
        if (
          element.condition.conditionValue ||
          element.condition.operator == "isNull" ||
          element.condition.operator == "isNotNull"
        ) {
          conditions.push({
            Field: element.dataField,
            Operator: element.condition.operator,
            Value: element.condition.conditionValue,
          });
        }
      });
      return {
        filterConditions: conditions,
        sortCondition: this.sortCondition,
        keyword: this.keyword,
        pageNumber: this.pageValue.pageNumber,
        pageSize: this.pageValue.pageSize,
      };
    },
    filteredColumns() {
      return this.columns.reduce((result, col, index) => {
        if (
          col.condition.conditionValue ||
          col.condition.operator == "isNull" ||
          col.condition.operator == "isNotNull"
        ) {
          result.push({
            Title: col.title,
            Field: col.dataField,
            Operator: col.condition.operator,
            Value: col.condition.conditionValue,
            Index: index,
          });
        }
        return result;
      }, []);
    },
    optionOperator() {
      var conditionType =
        this.columns[this.selectedColumnFilter].condition.type;
      switch (conditionType) {
        case "contains":
          return [
            { text: "Chứa", value: "contains" },
            { text: "Bắt đầu với", value: "startWith" },
            { text: "Kết thúc với", value: "endWith" },
          ];
        case "date":
          return [
            { text: "Bằng", value: "equal" },
            { text: "Lớn hơn", value: "bigger" },
            { text: "Nhỏ hơn", value: "smaller" },
          ];
        default:
          return null;
      }
    },
  },
  methods: {
    keydownList(event) {
      if (event.ctrlKey && event.keyCode === 49) {
        // 49 là mã ASCII của phím số 1
        // Thực hiện hành động tương ứng ở đây
        event.preventDefault();
        this.openPopupDetail({});
      }
    },
    /**
     * Lấy dữ liệu
     * Author: Nguyễn Văn Huy(03/04/2023)
     */
    async loadData() {
      this.showLoading = true;
      var filterQuery = this.filterQuery;
      try {
        var url = this.HConfig.API.Filter;
        await this.axios.post(url, filterQuery).then((response) => {
          if (response.data.Success) {
            this.employees = response.data.Data.Data;
            this.totalPages = response.data.Data.TotalPages;
            this.totalRecords = response.data.Data.TotalRecords;
            this.showLoading = false;
            if (
              this.pageValue.pageNumber > this.totalPages &&
              this.totalPages > 0
            ) {
              this.pageValue.pageNumber = this.totalPages;
              this.loadData();
            }
          } else {
            this.errorMessage = response.data.UserMsg;
            this.dialogType = this.HEnum.DialogType.Error;
          }
        });
      } catch (error) {
        this.errorMessage = this.HResource.Text.MessageException;
        this.dialogType = this.HEnum.DialogType.Error;
      }
    },
    /**
     * Bỏ lọc cột
     * @param {*} index
     * Author: Nguyễn Văn Huy (02/04/2023)
     */
    unFilter(index) {
      if (index != null) {
        this.columns[index].condition.conditionValue = null;
        if (this.columns[index].condition.type == "contains") {
          this.columns[index].condition.operator = "contains";
        } else if (this.columns[index].condition.type == "date") {
          this.columns[index].condition.operator = "equal";
        }
      } else {
        this.columns.forEach(function (col) {
          col.condition.conditionValue = null;
          if (col.condition.type == "contains") {
            col.condition.operator = "contains";
          } else if (col.condition.type == "date") {
            col.condition.operator = "equal";
          }
        });
      }
      this.loadData();
    },

    /**
     * Điều hướng sự kiện của HTable
     * Author: Nguyễn Văn Huy (1/3/2023)
     */
    eventTable(key, data) {
      switch (key) {
        case "delete":
          this.displayDeleteWarningPopup(
            this.HResource.Message.WarningDeleteRecord(data.EmployeeCode),
            data
          );
          break;
        case "openEditPopup":
          this.editEmployee(data);
          break;
        case "filterColumn":
          this.openFilterBox(data);
          break;
        case "clone":
          delete data["EmployeeId"];
          this.displayEmployeeDetail(data);
          break;
        case "refresh":
          this.loadData();
          break;
        default:
          break;
      }
    },

    /**
     * Mở hộp Filter cột
     * Author: Nguyễn Văn Huy(29/03/2023)
     */
    // openFilterBox(data) {
    //   this.filterBoxPosition = data.position - 302;
    //   if (
    //     this.selectedColumnFilter != data.index &&
    //     this.selectedColumnFilter != -1
    //   ) {
    //     this.allowCloseFilterBox = false;
    //   }
    //   this.selectedColumnFilter = data.index;
    //   this.txtFilter =
    //     this.columns[this.selectedColumnFilter].condition.conditionValue;
    // },

    /**
     * Click xoá nhiều bản ghi
     * Author: Nguyễn Văn Huy (28/03/2023)
     */
    onClickMultiDelete() {
      this.displayDeleteWarningPopup(
        this.HResource.Message.WarningMulteDelete(this.selectedListIds.length)
      );
    },

    /**
     * Điều hướng sự kiện của EmployeeDetail
     * Author: Nguyễn Văn Huy (1/3/2023)
     */
    eventDetail(key, data) {
      switch (key) {
        case "toTitleAdd":
          this.popupTitle = this.HResource.Title.FormAdd;
          break;
        case "showToast":
          this.$refs.toast.showToast(data);
          break;
        case "refresh":
          this.loadData();
          break;
        default:
          this.isShowPopup = false;
      }
    },

    /**
     * Hiển thị form Chi tiết nhân viên
     * Author: Nguyễn Văn Huy (1/3/2023)
     */
    displayEmployeeDetail(employee) {
      this.employeeSelected = employee;
      this.isShowPopup = true;
    },

    /**
     * Hiển thị form thêm mới nhân viên
     * Author: Nguyễn Văn Huy (08/03/2023)
     */
    openPopupDetail(employee) {
      this.popupTitle = "Thêm mới nhân viên";
      this.employeeSelected = {};
      this.displayEmployeeDetail(employee);
    },

    /**
     * Hiển thị form sửa thông tin nhân viên
     * Author: Nguyễn Văn Huy (08/03/2023)
     */
    editEmployee(employee) {
      this.popupTitle = "Sửa thông tin nhân viên";
      this.displayEmployeeDetail(employee);
    },

    /**
     * Xóa bản ghi
     * Author: Văn Huy (07/02/2023)
     */
    deleteData() {
      if (this.employeeTargetId) {
        this.deleteOneRecord();
        this.removeSelectedId(this.employeeTargetId);
        this.employeeTargetId = null;
      } else if (this.selectedListIds) {
        this.multiDelete();
      }
      this.errorMessage = null;
    },

    /**
     * Xóa 1 bản ghi theo id
     * Author: Văn Huy (07/02/2023)
     */
    removeSelectedId(id) {
      const index = this.selectedListIds.indexOf(id);
      if (index !== -1) {
        this.selectedListIds.splice(index, 1);
      }
    },
    deleteOneRecord() {
      try {
        this.axios
          .delete(this.HConfig.API.Employees + this.employeeTargetId)
          .then((response) => {
            if (response.data.Success) {
              this.$refs.toast.showToast(response.data.UserMsg);
              this.removeSelectedId(this.employeeSelected);
              this.loadData();
            } else {
              this.$refs.toast.showToast(response.data.UserMsg, "error");
            }
          });
      } catch (error) {
        this.errorMessage = error;
        this.dialogType = this.HEnum.DialogType.Error;
      }
    },

    /**
     * Xoá nhiều nhân viên
     * Author: Nguyễn Văn Huy (1/3/2023)
     */
    async multiDelete() {
      var url = this.HConfig.API.Employees;
      var data = this.selectedListIds;
      try {
        await this.axios({
          url: url,
          method: "DELETE",
          data: data,
        }).then((response) => {
          if (response.data.Success) {
            this.$refs.toast.showToast(response.data.UserMsg);
            this.selectedListIds = [];
            this.loadData();
          } else {
            this.$refs.toast.showToast(response.data.UserMsg, "error");
          }
        });
      } catch (error) {
        this.errorMessage = this.HResource.Text.MessageException;
        this.dialogType = this.HEnum.DialogType.Error;
      }
    },

    /**
     * Search dữ liệu theo keyword
     * Author: Nguyễn Văn Huy (28/03/2023)
     */
    search() {
      this.keyword = this.$refs.txtSearch.value;
      this.pageValue.pageNumber = 1;
      this.loadData();
    },

    /**
     * Đóng form Thêm nhân viên
     * Author: Nguyễn Văn Huy (1/3/2023)
     */
    closePopup() {
      this.isShowPopup = false;
    },

    /**
     * Xuất dữ liệu ra file Excel
     * Author: Nguyễn Văn Huy (1/3/2023)
     */
    async exportExcel() {
      this.showLoading = true;
      var filterQuery = this.filterQuery;
      await this.axios
        .post(
          `https://localhost:8888/api/v1/Employees/ExportExcel`,
          filterQuery,
          { responseType: "blob" }
        )
        .then((response) => {
          const blob = new Blob([response.data], {
            type: "application/vnd.ms-excel",
          });
          const fileName = "Danh_Sach_Nhan_Vien.xlsx"; // tên mặc định
          saveAs(blob, fileName);
        })
        .catch(() => {
          this.errorMessage = this.HResource.MessageException;
          this.dialogType = this.HEnum.DialogType.Error;
        });
      this.showLoading = false;
    },

    /**
     * Hiển thị Popup cảnh báo trước khi xoá nhân viên
     * Author: Nguyễn Văn Huy (1/3/2023)
     */
    displayDeleteWarningPopup(text, item) {
      if (item) {
        this.employeeTargetId = item["EmployeeId"];
      }
      this.dialogType = this.HEnum.DialogType.Warning;
      this.errorMessage = text;
    },

    /**
     * Sự kiện cho component Dialog
     * Author: Nguyễn Văn Huy(01/04/2023)
     */
    dialogEvent(key) {
      switch (key) {
        case "close":
          this.errorMessage = null;
          break;
        case "cancel":
          this.errorMessage = null;
          this.dialogType = this.HEnum.DialogType.Error;
          break;
        case "no":
          this.errorMessage = null;
          this.dialogType = this.HEnum.DialogType.Error;
          break;
        case "yes":
          this.deleteData();
          this.dialogType = this.HEnum.DialogType.Error;
          break;
        default:
          break;
      }
    },
  },
  data() {
    return {
      txtFilter: "",
      toast: {},
      dialogType: this.HEnum.DialogType.Error,
      selectedColumnFilter: -1,
      allowCloseFilterBox: true,
      filterBoxPosition: 0,
      showLoading: false,
      sortCondition: {},
      errorMessage: null,
      isShowDropList: false,
      selectedListIds: [],
      keyword: null,
      pageValue: {
        pageNumber: 1,
        pageSize: 20,
      },
      totalPages: 0,
      totalRecords: 0,
      employeeTargetId: "",
      popupTitle: "",
      employees: [],
      isShowPopup: false,
      showDelPopup: false,
      isShowDeletePopup: false,
      employeeSelected: {},
      columns: [],
      tableActions: [
        { name: "Nhân bản", key: "clone" },
        { name: "Xoá", key: "delete" },
      ],
    };
  },
};
</script>
  
<style scoped>
@import url(@/css/layout/content.css);
@import url(@/css/layout/datatables.css);
</style>