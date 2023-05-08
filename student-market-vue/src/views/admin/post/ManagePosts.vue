<template>
  <div class="av__container">
    <div class="av__title">
      <div>
        <h3>Quản lý nhân viên</h3>
      </div>
      <HButton
        @click="openPopupDetail"
        value="Thêm mới nhân viên"
        type="btn-pri"
      />
    </div>
    <div class="ac__content">
      <div class="table__top">
        <div class="table__top-left">
          <div>
            <button
              class="btn-multiaction"
              :disabled="!isShowMultiDelete"
              @click="isShowDropList = true"
            >
              Thực hiện hàng loạt
              <h-icon position="-564px -365px" size="8 5" />
            </button>
            <div
              v-if="isShowDropList"
              class="dropdownmulti dropdownlist"
              v-click-outside="() => (isShowDropList = false)"
            >
              <ul>
                <li
                  v-for="(actionItem, indexAction) in multiactions"
                  :key="indexAction"
                >
                  <input
                    class="btn-link"
                    type="button"
                    :value="actionItem.name"
                    @click="
                      selectAction = -1;
                      onClickMultiAction(actionItem.key);
                    "
                  />
                </li>
              </ul>
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
          <div class="searchbox">
            <h-input
              ref="txtSearch"
              v-model="txtSearch"
              @keyup.enter="filterData"
              placeholder="Tìm kiếm"
            />
            <div class="icon-search" @click="onSearch">
              <h-icon position="-956px -31px" size="22 22" />
            </div>
          </div>
        </div>
      </div>
      <HTable
        class="content__table"
        v-model="selectedListIds"
        idField="PostID"
        :modDetail="true"
        :columns="columns"
        :data="Posts"
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
    <PopupPostDetail
      v-if="isShowPopup"
      @eventDetail="eventDetail"
      :PostId="PostSelected"
    ></PopupPostDetail>
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
import PopupPostDetail from "./PopupPostDetail.vue";
import { saveAs } from "file-saver";
export default {
  name: "ManagePosts",
  components: { PopupPostDetail },
  created() {
    this.columns = this.HConfig.ListPostColumns;
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
    onClickMultiAction(key) {
      switch (key) {
        case "delete":
          this.onClickMultiDelete();
      }
    },
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
     * Author: Nguyễn Văn Huy(03/04/2023)this.HConfig.API.Filter
     */
    async loadData() {
      this.showLoading = true;
      var filterQuery = this.filterQuery;
      try {
        var url = "https://localhost:9999/api/v1/Posts/Filter";
        await this.axios.post(url, filterQuery).then((response) => {
          if (response.data.Success) {
            this.Posts = response.data.Data.Data;
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
            this.errorMessage = response.data.PostMsg;
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
            this.HResource.Message.WarningDeleteRecord(data.PostCode),
            data
          );
          break;
        case "openEditPopup":
          this.displayPost(data);
          break;
        case "displayDetail":
          this.displayPost(data);
          break;
        case "filterColumn":
          this.openFilterBox(data);
          break;
        case "refresh":
          this.loadData();
          break;
        case "approved":
          this.approvedPost(data, true);
          break;
        case "refuse":
          this.approvedPost(data, false);
          break;
        default:
          break;
      }
    },

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
     * Điều hướng sự kiện của PostDetail
     * Author: Nguyễn Văn Huy (1/3/2023)
     */
    eventDetail(key, data) {
      switch (key) {
        case "approved":
          this.approvedPost(data, true);
          break;
        case "refuse":
          this.approvedPost(data, false);
          break;
        case "refresh":
          this.loadData();
          break;
        default:
          this.isShowPopup = false;
      }
    },

    /**
     * Thêm hoặc sửa bản ghi
     * Author: Văn Huy (16/03/2023)
     */
    async approvedPost(item, approved) {
      var p = item;
      var url = this.HConfig.API.Posts;
      if (approved) {
        url = url + "Approved/" + p.PostID + `?approved=${1}`;
      } else {
        url = url + "Approved/" + p.PostID + `?approved=${2}`;
      }
      if (!this.errorMessage) {
        try {
          await this.axios.put(url).then((response) => {
            if (response.data.Success) {
              this.$emit("eventDetail", "showToast", response.data.UserMsg);
              this.$emit("eventDetail", "refresh");
              item.Approved = approved ? 1 : 2;
              this.closePopup();
            } else {
              this.errorMessage = response.data.UserMsg;
            }
          });
        } catch (error) {
          this.errorMessage = this.HResource.Message.Exception;
        }
      }
    },

    /**
     * Hiển thị form Chi tiết nhân viên
     * Author: Nguyễn Văn Huy (1/3/2023)
     */
    displayPostDetail(PostID) {
      this.PostSelected = PostID;
      this.isShowPopup = true;
    },

    /**
     * Hiển thị form thêm mới nhân viên
     * Author: Nguyễn Văn Huy (08/03/2023)
     */
    openPopupDetail(Post) {
      this.popupTitle = "Thêm mới nhân viên";
      this.PostSelected = {};
      this.displayPostDetail(Post);
    },

    /**
     * Hiển thị form sửa thông tin nhân viên
     * Author: Nguyễn Văn Huy (08/03/2023)
     */
    displayPost(Post) {
      this.popupTitle = "Sửa thông tin nhân viên";
      this.displayPostDetail(Post.PostID);
    },

    /**
     * Xóa bản ghi
     * Author: Văn Huy (07/02/2023)
     */
    deleteData() {
      if (this.PostTargetId) {
        this.deleteOneRecord();
        this.removeSelectedId(this.PostTargetId);
        this.PostTargetId = null;
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
          .delete(this.HConfig.API.Posts + this.PostTargetId)
          .then((response) => {
            if (response.data.Success) {
              this.$refs.toast.showToast(response.data.Data.UserMsg);
              this.removeSelectedId(this.PostSelected);
              this.loadData();
            } else {
              this.$refs.toast.showToast(response.data.Data.UserMsg, "error");
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
      var url = this.HConfig.API.Posts;
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
        .post(`https://localhost:8888/api/v1/Posts/ExportExcel`, filterQuery, {
          responseType: "blob",
        })
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
        this.PostTargetId = item["PostId"];
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
      dialogType: this.HEnum.DialogType.Warning,
      selectedColumnFilter: -1,
      allowCloseFilterBox: true,
      filterBoxPosition: 0,
      showLoading: true,
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
      PostTargetId: "",
      popupTitle: "",
      Posts: [],
      isShowPopup: false,
      showDelPopup: false,
      isShowDeletePopup: false,
      PostSelected: "",
      columns: [],
      tableActions: [
        { name: "Phê Duyệt", key: "approved" },
        { name: "Từ chối", key: "refuse" },
        { name: "Xoá", key: "delete" },
      ],
      multiactions: [{ name: "Xoá", key: "delete" }],
    };
  },
};
</script>
      
    <style >
/* @import url(@/css/layout/content.css);
  @import url(@/css/layout/datatables.css); */
@import url(@/css/views/userlist.css);
@import url(@/css/views/managepost.css);
</style>