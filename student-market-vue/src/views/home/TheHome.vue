<template>
  <div class="the-home">
    <div class="filter-top shadow-sm">
      <h-combobox
        ref="cbbCategories"
        :api="apiGetCategories"
        :defaultItem="defaultCategory"
        propText="CategoryName"
        propValue="CategoryID"
        v-model="category_id"
      ></h-combobox>
      <h-combobox
        ref="cbbLocations"
        :api="apiGetLocations"
        :defaultItem="defaultLocation"
        propText="LocationName"
        propValue="LocationID"
        v-model="location_id"
      ></h-combobox>
      <h-combobox
        :data="sortDataCombobox"
        :defaultItem="sortDefault"
        propText="text"
        propValue="value"
        v-model="sortType"
      ></h-combobox>
      <div class="searchbox">
        <h-input
          ref="txtSearch"
          v-model="txtSearch"
          @keyup.enter="loadData"
          placeholder="Tìm kiếm"
        />
        <div class="icon-search" @click="onSearch">
          <h-icon position="-956px -31px" size="22 22" />
        </div>
      </div>
      <HButton
        ref="btnFilter"
        value="Lọc"
        type="btn-pri"
        :disabled="false"
        @click="loadData"
      />
    </div>
    <h-post-list :posts="posts" />
    <div class="paging-box">
      <h-paging
        :page-count="totalPages"
        :click-handler="loadData"
        prev-text="&#10096;"
        next-text="&#10097;"
        :container-class="'pagination'"
        v-model="pageNumber"
      ></h-paging>
    </div>
    <h-loading v-show="showLoading"></h-loading>
  </div>
</template>

<script>
import HPostList from "@/components/postlist/HPostList.vue";
import { eventBus } from "@/js/eventbus";
import { useKeyword } from "@/stores/keyword";
import HCombobox from "@/components/combobox/HCombobox.vue";
export default {
  components: { HPostList, HCombobox },
  name: "TheHome",
  async created() {
    this.loadData();
  },
  watch: {
    category_id: function () {
      this.loadData();
    },
    location_id: function () {
      this.loadData();
    },
  },
  methods: {
    /**
     * Lấy dữ liệu
     * Author: Nguyễn Văn Huy(03/04/2023)
     */
    async loadData() {
      this.showLoading = true;

      var filterQuery = await this.filterQuery();
      try {
        var url = "https://localhost:9999/api/v1/Posts/Search";
        await this.axios.post(url, filterQuery).then((response) => {
          if (response.data.Success) {
            this.posts = response.data.Data.Data;
            this.totalPages = response.data.Data.TotalPages;
            this.totalRecords = response.data.Data.TotalRecords;
            this.showLoading = false;
            if (this.pageNumber > this.totalPages && this.totalPages > 0) {
              this.pageNumber = this.totalPages;
              this.loadData();
            }
          } else {
            this.errorMessage = response.data.UserMsg;
            this.dialogType = this.HEnum.DialogType.Error;
          }
        });
      } catch (error) {
        this.errorMessage = error;
      }
    },
    loadKeyword() {
      const keywordStore = useKeyword();
      eventBus.on("search", (keyword) => {
        keywordStore.setKeyword(keyword);
        this.keyword = keyword;
        this.loadData();
      });
    },
    filterQuery() {
      var result = {
        Keyword: this.txtSearch,
        CategoryID: this.category_id,
        LocationID: this.location_id,
        SortType: this.sortType,
        PageNumber: this.pageNumber,
      };
      return result;
    },
  },
  mounted() {
    const keywordStore = useKeyword();
    eventBus.on("search", (keyword) => {
      keywordStore.setKeyword(keyword);
      this.keyword = keyword;
      this.loadData();
      this.$router.push("/trang-chu");
    });
  },
  data() {
    return {
      keyword: null,
      showLoading: true,
      pageNumber: 1,
      totalPages: 0,
      apiGetCategories: "https://localhost:9999/api/v1/Categories",
      category_id: null,
      defaultCategory: {
        CategoryID: null,
        CategoryName: "Chọn danh mục",
      },
      apiGetLocations: "https://localhost:9999/api/v1/Locations",
      location_id: null,
      defaultLocation: {
        LocationID: null,
        LocationName: "Chọn khu vực",
      },
      sortDefault: { text: "Mới nhất", value: 0 },
      sortDataCombobox: [
        { text: "Giá tăng dần", value: 1 },
        { text: "Giá giảm dần", value: 2 },
      ],
      posts: [],
    };
  },
};
</script>

<style scope>
@import url(@/css/views/home.css);
</style>