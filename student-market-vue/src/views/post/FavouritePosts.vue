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
    <h-loading v-show="showLoading"></h-loading>
  </div>
</template>
  
  <script>
import HPostList from "@/components/postlist/HPostList.vue";
import { eventBus } from "@/js/eventbus";
import { useKeyword } from "@/stores/keyword";
import { getUserFromLocalStorage } from "@/stores/localStorage";
import HCombobox from "@/components/combobox/HCombobox.vue";
export default {
  components: { HPostList, HCombobox },
  name: "TheHome",
  async created() {
    this.user = await getUserFromLocalStorage();
    await this.loadData();
  },
  watch: {
    category_id: function () {
      this.loadData();
    },
    location_id: function () {
      this.loadData();
    },
    sortType: function () {
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
        var url = this.HConfig.API.FavouritePosts + this.user.UserID;
        await this.axios.post(url, filterQuery).then((response) => {
          if (response.data.Success) {
            this.posts = response.data.Data;
            this.showLoading = false;
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
      user: {},
      keyword: null,
      sortType: 0,
      showLoading: true,
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