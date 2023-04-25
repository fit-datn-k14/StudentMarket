<template>
  <div class="product-detail-view"></div>
</template>
  
  <script>
import HGridTree from "@/components/table/HGridTree.vue";
export default {
  components: { HGridTree },
  name: "TheAccounts",
  async created() {
    await this.loadData();
  },
  methods: {
    async loadData() {
      this.showLoading = true;
      try {
        var url = "https://localhost:8888/api/v1/Accounts/";
        await this.axios.get(url).then((response) => {
          if (response.data.Success) {
            this.accounts = response.data.Data;
            this.showLoading = false;
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
  },
  data() {
    return {
      accounts: [],
      columns: [
        {
          title: "Số tài khoản",
          dataField: "AccountNumber",
          width: "160px",
        },
        {
          title: "Tên Tài Khoản",
          dataField: "AccountName",
          width: "200px",
        },
        {
          title: "Diễn giải",
          dataField: "Description",
          width: "140px",
        },
        {
          title: "Tên Tiếng Anh",
          dataField: "EnglishName",
          width: "140px",
        },
      ],

      tableActions: [
        { name: "Nhân bản", key: "clone" },
        { name: "Xoá", key: "delete" },
      ],
    };
  },
};
</script>
  
  <style>
</style>