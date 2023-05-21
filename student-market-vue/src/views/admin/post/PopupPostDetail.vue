<template>
  <div ref="formPostDetail" class="popup_add_user" @keydown="keydownDetail">
    <div class="bgshadow" tabindex="0"></div>
    <div class="popup" tabindex="0">
      <div class="icon-top-right-popup">
        <h-icon
          class="icon popup__btn-close"
          position="-89px -145px"
          size="22 22"
          title="Hỗ trợ"
        />
        <h-icon
          class="icon popup__btn-close"
          position="-147px -147px"
          size="18 18"
          title="Đóng(ESC)"
          @click="onClickClose"
        />
      </div>
      <div class="popup__title">
        <div class="popup__title-text">Chi Tiết Tin Đăng</div>
      </div>
      <div class="popup__content">
        <div class="row col-md-12">
          <div class="col-md-6">
            <img
              class="ppd__img border border-dark"
              :src="URL + `Images/posts/${PostId}/${ImageID}`"
            />
            <div class="imglist">
              <img
                v-for="(imgID, indexImg) in Post.ListImages"
                :key="indexImg"
                :src="URL + `Images/posts/${PostId}/${imgID}`"
                @click="ImageID = imgID"
              />
            </div>
          </div>
          <div class="col-md-6 ppd__info">
            <div class="ppd__title">
              {{ Post.Title }}
            </div>
            <div v-if="Post.Price" class="ppd__price">
              {{ HCommon.formatMoney(Post.Price) }}
            </div>
            <div v-else class="ppd__price">Liên hệ</div>

            <div class="row">
              <label class="col-md-6" for="">Danh mục:</label>
              <div class="ppd__describe col-md-6">
                <p>{{ Post.CategoryName }}</p>
              </div>
            </div>
            <div class="row">
              <label class="col-md-6" for="">Khu vực:</label>
              <div class="ppd__describe col-md-6">
                <p>{{ Post.LocationName }}</p>
              </div>
            </div>
            <label class="col-md-6" for="">Địa chỉ:</label>
            <div class="ppd__describe col-md-6">
              <p>{{ Post.LocationName }}</p>
            </div>
            <label for="">Mô tả:</label>
            <div v-if="Post.PostDescribe" class="ppd__describe">
              <p class="clamp-12">{{ Post.PostDescribe }}</p>
            </div>
            <div v-else class="ppd__describe">
              <p>Không có mô tả</p>
            </div>
          </div>
        </div>
      </div>
      <div class="popup__footer">
        <div>
          <HButton
            type="btn-second"
            ref="btnRefuse"
            value="Từ Chối"
            @click="approvedPost(false)"
          />
          <HButton
            type="btn-pri"
            id="btnApproved"
            value="Phê Duyệt"
            @click="approvedPost(true)"
          ></HButton>
        </div>
        <div>
          <HButton
            type="btn-second"
            @click="closePopup"
            ref="btnCancel"
            value="Huỷ"
            @keydown.tab.prevent="focusRefuse"
          />
        </div>
      </div>
    </div>
    <h-dialog
      v-if="errorMessage"
      :content="errorMessage"
      :type="dialogType"
      @dialogEvent="dialogEvent"
    />
    <h-loading v-if="isLoading"></h-loading>
  </div>
</template>
  
  <script>
import HConfig from "@/js/base/config";
export default {
  name: "PopupPostDetail",
  props: {
    modelValue: [Object],
    PostId: {
      type: Object,
      required: false,
    },
  },
  async created() {
    await this.loadData();
    this.ImageID = this.Post.ImageName;
  },
  mounted() {
    this.focusRefuse();
  },
  methods: {
    focusRefuse() {
      this.$refs.btnRefuse.$refs.hbutton.focus();
    },
    /**
     * Sự kiện nhấn phím tắt cho form chi tiết
     * Author: huynv (02/03/2023)
     */
    keydownDetail(event) {
      event.stopPropagation();
      if (event.key === "Escape") {
        this.onClickClose();
      } else if (event.ctrlKey && event.shiftKey && event.keyCode === 83) {
        this.approvedPost(true);
      } else if (event.ctrlKey && event.keyCode === 83) {
        event.preventDefault();
        this.approvedPost(false);
      }
    },

    async loadData() {
      this.showLoading = true;
      try {
        var url = HConfig.API.Posts + this.PostId;
        await this.axios.get(url).then((response) => {
          if (response.data.Success) {
            this.Post = response.data.Data;
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

    /**
     * Đóng popup
     * Author: huynv (02/03/2023)
     */
    closePopup() {
      this.$emit("eventDetail");
    },

    /**
     * Sự kiện cho component Dialog
     * Author: Nguyễn Văn Huy(01/04/2023)
     */
    dialogEvent(key) {
      this.errorMessage = null;
      switch (key) {
        case "close":
          break;
        default:
          break;
      }
    },

    /**
     * Sự kiện khi click nút đóng
     * Author: Nguyễn Văn Huy(30/03/2023)
     */
    onClickClose() {
      this.closePopup();
    },

    /**
     * Thêm hoặc sửa bản ghi
     * Author: Văn Huy (16/03/2023)
     */
    async approvedPost(approved) {
      if (approved) {
        this.$emit("eventDetail", "approved", this.Post);
      } else {
        this.$emit("eventDetail", "refuse", this.Post);
      }
    },

    showErrorMessage(str) {
      this.errorMessage = str;
    },
  },
  data() {
    return {
      URL: HConfig.URL,
      ImageID: null,
      isLoading: false,
      dialogType: this.HEnum.DialogType.Error,
      errorMessage: "",
      Post: {},
    };
  },
};
</script>
  
  <style scoped>
@import url(@/css/views/admin/popuppostdetail.css);
</style>