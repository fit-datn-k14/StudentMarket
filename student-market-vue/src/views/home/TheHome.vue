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
        propValue="LocationsID"
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
          @keyup.enter="filterData"
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
        @click="filterData"
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
    <h-loading v-if="showLoading"></h-loading>
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
    category_id() {
      this.loadData();
    },
    location_id() {
      this.loadData();
    },
  },
  methods: {
    filterData() {},
    /**
     * Lấy dữ liệu
     * Author: Nguyễn Văn Huy(03/04/2023)
     */
    async loadData() {
      this.showLoading = true;
      var filterQuery = this.filterQuery;
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
        this.errorMessage = this.HResource.Text.MessageException;
        this.dialogType = this.HEnum.DialogType.Error;
      }
    },
    loadKeyword() {
      const keywordStore = useKeyword();
      eventBus.on("search", (keyword) => {
        keywordStore.setKeyword(keyword);
      });
    },
  },
  mounted() {
    const keywordStore = useKeyword();
    eventBus.on("search", (keyword) => {
      keywordStore.setKeyword(keyword);
      this.$router.push("/Home");
    });
  },
  computed: {
    filterQuery() {
      return {
        Keyword: this.keyword,
        CategoryID: this.category_id,
        LocationID: this.location_id,
        SortType: this.sortType,
        PageNumber: this.pageNumber,
      };
    },
  },
  data() {
    return {
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
      posts: [
        {
          PostID: "598eb899-611e-226b-5acb-b8d37b68af4d",
          PostCode: "PO-37825",
          Title: "Libero numquam qui sit expedita ullam.",
          Describe:
            "Quibusdam tempora quo nam aut aspernatur molestiae sequi ipsum minus sint error quod unde error. Deleniti ipsa, neque a rem et optio voluptas eveniet eum minus maiores et repudiandae placeat? Dolorum voluptatem ut omnis ullam perspiciatis et quas corrupti earum unde ullam sed explicabo officia. Molestiae natus saepe quidem et dolores, tenetur voluptatum consequuntur vero error sit natus veniam nulla. Hic tempora aliquam officiis, modi est nobis quis tempora non in eos laudantium consequatur dicta. Ut quisquam voluptatem aut quam consectetur est quasi officiis et ipsam omnis neque sunt voluptas. Veritatis et saepe a totam eos repellat omnis et voluptas repudiandae laudantium non dolorem nobis. Molestiae atque deserunt sunt reiciendis temporibus dolores iusto soluta voluptas, quia quasi enim molestiae maiores! Nisi et est iusto minus ad suscipit, sed sint et ut exercitationem quae labore omnis. Accusamus dolores facere non consectetur obcaecati voluptatem nostrum placeat et ratione amet ipsa eos dolores. Incidunt error esse natus similique beatae voluptas nisi repellat qui sit ipsum sequi nam ut; totam vero eveniet sit autem molestias esse sit rem quas totam unde eveniet aut vitae. Voluptas voluptatem dolorem iste ipsam ex illo adipisci dolores tenetur nisi unde et similique omnis! Dolorem voluptas, commodi atque id neque unde aut molestiae ratione aperiam cupiditate magni debitis nesciunt? Libero repudiandae neque expedita necessitatibus totam accusantium unde nostrum unde ab consectetur qui obcaecati voluptate. Sit qui non natus, omnis perferendis alias consequuntur asperiores vitae et enim omnis architecto sed. Tempora ipsam in omnis debitis ullam, distinctio exercitationem unde ut amet id obcaecati nemo consequatur. Sed doloribus ipsam eos ipsum sequi qui reprehenderit consectetur itaque qui sit optio aut expedita!\r\nUt sed eos eius hic in autem et eaque corporis dolorem sed mollitia accusantium aut. Omnis ut, nihil id ratione voluptatem dolore quasi recusandae aliquid autem nihil aliquid voluptatibus aspernatur. Et nam sit cum explicabo voluptas deleniti non enim veniam beatae eius maiores ipsa consequatur. Repellendus recusandae eos, ut quo odio consequatur autem architecto nobis dolorem eius cum deleniti aut. Consequatur officiis quo obcaecati fugiat sunt pariatur qui voluptatem explicabo natus a error natus impedit! Molestiae ad quam, fugit dolorem facilis quia rerum aut suscipit voluptate velit omnis dolor et. Laboriosam dolores atque ut vel in est voluptatem ipsa facere inventore magnam eius id ut. Quia neque, voluptatem ex similique debitis similique illum enim iusto magni ratione omnis magni quisquam! Mollitia obcaecati autem et labore, aliquid voluptate enim ut eveniet ad natus exercitationem molestias reprehenderit; neque molestias ab illo iure doloremque eos quae quisquam labore distinctio quae tempora error quisquam. Rerum laudantium, unde sit nobis quam deleniti accusantium qui labore voluptatem deserunt quaerat minus expedita. Consequatur voluptatem perspiciatis distinctio, obcaecati temporibus sit quia commodi autem aut quo odit molestiae sed. Enim deleniti voluptatem sint, modi officiis nihil consectetur explicabo adipisci iste qui beatae numquam doloremque; eos velit fuga iure veritatis quis ipsa voluptatum at obcaecati id quisquam neque itaque debitis...",
          Approved: 0,
          UserID: "39446ba3-3071-7af6-a5a9-deaf32c96293",
          CategoryID: "2bbbd485-6df2-536a-7e32-6a1673ffca7c",
          CreatedDate: "2023-03-25T09:01:29",
          CreatedBy: "Nông Văn Thịnh",
          ModifiedDate: "2023-03-25T09:01:29",
          ModifiedBy: "Hoàng Văn Hạnh",
          FullName: "Nguyễn Ánh Thư",
        },
        {
          PostID: "59c4865b-1492-3701-4469-9f77bda75690",
          PostCode: "PO-52628",
          Title: "Corrupti sit sed.",
          Describe:
            "Dignissimos voluptatem nihil, non enim neque ut aut deleniti quibusdam assumenda minima voluptatem doloribus quam. Aspernatur iste expedita, aut perspiciatis ex magni unde eum laborum optio facere et neque laboriosam. Et in reprehenderit repudiandae laudantium voluptates neque perferendis, fuga eum quia non corporis qui a. Excepturi sit quia sequi et doloribus consequatur nulla quia sit ad assumenda maxime natus consequuntur. Animi necessitatibus consequuntur iste aliquid dolor quasi perferendis assumenda sit aspernatur veritatis qui vitae excepturi!\r\nNeque sed at quia nisi natus inventore voluptate nihil quo cupiditate impedit error voluptas nobis. Et ut doloribus, harum modi magnam ut sequi tempora consequatur dolorem sit qui voluptatibus nisi. Exercitationem perspiciatis qui ullam debitis et cupiditate deserunt et, quos perferendis quis error velit exercitationem. Nisi et debitis nobis quae rerum autem dolore officiis iusto rerum voluptates enim expedita omnis. Et voluptas sequi, voluptatem sed numquam repudiandae aspernatur velit est esse aut aliquam amet eum. Dolorem aut quod repudiandae, omnis voluptatem id mollitia nihil aspernatur dolor aut consectetur quisquam hic. Dolore est, et quia odio voluptas labore exercitationem nisi ut consequuntur et perspiciatis maiores error. Vel neque iste nam rerum tempora dolor aperiam possimus eveniet debitis omnis sint et incidunt. Molestias consequatur voluptas excepturi consectetur quibusdam minima alias corporis est ut excepturi fuga eveniet possimus! Facere ipsum ut, laudantium sit temporibus debitis sunt voluptatum quasi consectetur consequuntur consequatur quia laudantium. Vero consequatur inventore odit tenetur numquam architecto quia voluptatum aliquid saepe modi sed iste reiciendis; et laudantium nobis culpa cupiditate velit aut rem maiores suscipit eius magnam optio exercitationem unde. Qui dolorum, dolor quia doloribus quia ex expedita iste molestiae delectus quas sed consequatur incidunt. Et eligendi ut consequuntur eum odio et ut corrupti itaque corporis nostrum omnis molestiae neque...",
          Approved: 1,
          UserID: "174ffc6c-4080-4bc3-a126-71cca92946bf",
          CategoryID: "2bbbd485-6df2-536a-7e32-6a1673ffca7c",
          CreatedDate: "2023-03-25T09:01:29",
          CreatedBy: "Nguyễn Minh Đông",
          ModifiedDate: "2023-03-25T09:01:29",
          ModifiedBy: "Nguyễn Ánh Cảnh",
          FullName: "Tống Ánh Chi",
        },
        {
          PostID: "59c8e347-352c-11cc-4195-2c566fa88632",
          PostCode: "PO-64721",
          Title: "Perferendis dolorem est autem est.",
          Describe:
            "Porro provident commodi voluptate exercitationem qui sit ducimus voluptatem labore sit distinctio repudiandae quidem voluptates. Ut porro autem iste recusandae sint cum numquam quia distinctio unde architecto aliquid est magni. Molestias et distinctio sunt eveniet cum officiis corrupti blanditiis modi deleniti veniam qui natus veritatis! Provident ut harum dolor velit est, optio dolores sunt dicta minima quae quisquam est incidunt. Distinctio voluptatem quisquam qui voluptatem quae voluptatem consequatur ratione eum suscipit nulla illo tenetur ipsum? Aliquid nihil nam vero sed aut ducimus vero sunt aspernatur culpa eos, sed magnam omnis. Omnis molestiae odit repellendus tempore fugit nostrum est omnis error perspiciatis non voluptatibus error rerum. Minus corrupti voluptatem doloribus illum voluptas est enim aspernatur debitis ut laboriosam officia magni ipsa. Inventore eum minus quos esse accusantium sint cum porro, iste cum quod voluptatem temporibus ut! Consequatur nam eum ipsa sit dolore voluptates, qui a quos iste quod id earum et. Corrupti sit accusantium voluptates officiis dolorum soluta voluptatibus dolores voluptatem fuga qui adipisci voluptate perspiciatis. Et asperiores odio laudantium cupiditate velit, dolores doloremque omnis sunt vitae consequatur laborum in et! Veniam nulla in eaque autem obcaecati voluptatem vel et quo nihil ipsum consequatur laborum libero. Voluptatem aut labore a ut laboriosam fugit ipsa, quas quis voluptas sequi provident error eum.\r\nPerferendis sit veniam error sint sit magni nemo iste dolore dolor necessitatibus sit et vel. Asperiores similique, esse totam rem aliquid necessitatibus doloribus corporis aliquam non voluptatem officiis natus dolores.\r\nPossimus consectetur velit optio nisi ut natus sit quisquam nam eveniet dolore reiciendis neque et. Alias autem voluptatem necessitatibus omnis nemo incidunt minus nobis perspiciatis similique rerum distinctio delectus totam; odio natus iste tempore dolore voluptatem vero voluptates fugiat error, est hic quia commodi sit. Id ipsam nostrum reprehenderit quia sed nostrum velit iste voluptates eos ipsam voluptatibus quod et; sint soluta nisi sit dolores eveniet, ut accusamus perspiciatis voluptatem velit est voluptatum eveniet tenetur. Id autem repudiandae sit unde minus, adipisci omnis cum esse voluptatem veniam veritatis ut eligendi.",
          Approved: 1,
          UserID: "16c1b62f-1f00-1827-945e-60fe4d31c979",
          CategoryID: "5b2fcdf5-2050-7cb2-8f90-67245e8b283e",
          CreatedDate: "2023-03-25T09:01:29",
          CreatedBy: "Trần Thị Phương",
          ModifiedDate: "2023-03-25T09:01:29",
          ModifiedBy: "Nguyễn Ánh Vinh",
          FullName: "Nguyễn Linh Vinh",
        },
      ],
    };
  },
};
</script>

<style >
.the-home {
  display: flex;
  flex-direction: column;
  align-items: center;
  row-gap: 16px;
}

.filter-top {
  height: 48px;
  display: flex;
  justify-content: center;
  align-items: center;
  background-color: #fff;
  column-gap: 16px;
  width: 100%;
}
.filter-top > div {
  max-width: 240px;
  margin: 0;
}
.pagination {
  column-gap: 8px;
}
.page-item.active {
  border: none !important;
}

.page-item.disabled {
  opacity: 0.7;
}

.page-item.active .page-link {
  background-color: var(--primary-color);
  border-color: var(--primary-color);
}

.page-item > .page-link {
  color: var(--text-color);
  border-radius: 4px;
  width: 36px;
  box-sizing: border-box;
}

.page-item:not(.active, .disabled):hover > .page-link {
  background-color: #fff;
  color: var(--primary-color);
}
</style>