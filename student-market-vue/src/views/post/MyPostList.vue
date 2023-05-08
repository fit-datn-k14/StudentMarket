<template>
  <div class="my-post-list">
    <div class="filter-top shadow-sm">
      <div
        class="optionList"
        v-for="(option, index) in optionList"
        :key="index"
      >
        <h-button
          class="btn-option"
          :type="option.value == selectedOption ? 'btn-pri' : 'btn-second'"
          :value="option.text"
          @click="onSelectOption(option.value)"
        />
      </div>
      <div class="searchbox">
        <h-input
          ref="txtSearch"
          v-model="txtSearch"
          @keyup.enter="onSearch"
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
    <h-post-list class="mpl__postlist" :posts="filterPosts" />
  </div>
</template>

<script>
import HButton from "@/components/button/HButton.vue";
import { getUserFromLocalStorage } from "@/stores/localStorage";
export default {
  components: { HButton },
  name: "MyPostList",
  async created() {
    this.user = await getUserFromLocalStorage();
    await this.loadData();
  },
  methods: {
    onSelectOption(value) {
      this.selectedOption = value;
    },
    onSearch() {
      this.keyword = this.txtSearch ? this.txtSearch : "";
    },
    loadData() {
      try {
        var url = this.HConfig.API.Posts + "GetByUser/" + this.user.UserID;
        this.axios.get(url).then((response) => {
          if (response.data.Success) {
            this.posts = response.data.Data;
          } else {
            this.errorMessage = response.data.UserMsg;
          }
        });
      } catch {
        this.errorMessage = this.HResource.Text.MessageException;
      }
    },
  },
  computed: {
    filterPosts() {
      if (this.selectedOption != null) {
        return this.posts.filter(
          (p) =>
            p.Approved == this.selectedOption && p.Title.includes(this.keyword)
        );
      } else {
        return this.posts.filter((p) => p.Title.includes(this.keyword));
      }
    },
  },
  data() {
    return {
      user: {},
      posts: [],
      txtSearch: "",
      keyword: "",
      optionList: [
        { text: "Tất cả", value: null },
        { text: "Chưa duyệt", value: 0 },
        { text: "Đã Duyệt", value: 1 },
        { text: "Đã Từ chối", value: 2 },
      ],
      selectedOption: null,
    };
  },
};
</script>

<style>
.my-post-list {
  display: flex;
  flex-direction: column;
  align-items: center;
}
.mpl__postlist {
  margin-top: 60px;
}
.optionList > .btn-option input {
  border-radius: 18px !important;
}
</style>