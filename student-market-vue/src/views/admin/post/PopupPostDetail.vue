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
            <div v-if="Post.Price" class="ppd__price">{{ Post.Price }} đ</div>
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
              <p>{{ Post.PostDescribe }}</p>
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
            @click="updatePost(false)"
          />
          <HButton
            type="btn-pri"
            id="btnApproved"
            value="Phê Duyệt"
            @click="updatePost(true)"
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
    console.log(this.Post);
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
        this.updatePost(true);
      } else if (event.ctrlKey && event.keyCode === 83) {
        event.preventDefault();
        this.updatePost(false);
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
    async updatePost(approved) {
      var p = this.Post;
      var url = this.HConfig.API.Posts;
      if (approved) {
        p.Approved = 1;
      } else {
        p.Approved = 2;
      }
      if (!this.errorMessage) {
        url = url + p.PostID;
        try {
          await this.axios.put(url, p).then((response) => {
            if (response.data.Success) {
              this.$emit("eventDetail", "showToast", response.data.UserMsg);
              this.$emit("eventDetail", "refresh");
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
.icon-top-right-popup {
  position: absolute;
  top: 0;
  right: 0;
  display: flex;
  column-gap: 8px;
}
.popup_add_user {
  position: fixed;
  top: 0;
  right: 0;
  bottom: 0;
  left: 0;
}

.bgshadow {
  display: block;
  position: fixed;
  top: 0;
  right: 0;
  bottom: 0;
  left: 0;
  background-color: #00000050;
  z-index: 10;
}

.popup {
  width: 900px;
  position: fixed;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  background-color: #fff;
  border-radius: 4px;
  min-width: 360px;
  z-index: 10;
}

.popup * {
  font-family: Roboto;
}

.popup__btn-close {
  background: url(@/assets/img/Sprites.64af8f61.svg) no-repeat -143px -145px;
  position: absolute;
  width: 24px;
  height: 24px;
  border: none;
  top: 24px;
  right: 24px;
  cursor: pointer;
}

.popup__btn-help {
  background: url(@/assets/img/Sprites.64af8f61.svg) no-repeat -88px -145px;
  width: 24px;
  height: 24px;
  position: absolute;
  width: 24px;
  height: 24px;
  border: none;
  top: 24px;
  right: 60px;
  cursor: pointer;
}

.popup__title {
  padding: 24px 24px 0;
  box-sizing: border-box;
}

.popup__title-text {
  font-size: 24px;
  font-weight: 700;
}

.popup__content {
  padding: 24px;
  box-sizing: border-box;
}

.popup__footer {
  display: flex;
  flex-direction: row-reverse;
  justify-content: space-between;
  align-items: center;
  width: 100%;
  height: 60px;
  background-color: #f5f5f5;
  border-radius: 0 0 4px 4px;
  padding: 24px;
  box-sizing: border-box;
}

.popup__footer > div:first-child {
  display: flex;
  align-items: center;
  column-gap: 8px;
}

.popup__content {
  display: flex;
}

/* 
  ------------------------------------------------ */

.popup-add-employee {
  font-family: "Roboto";
}

.popup__text {
  width: 360px;
}

.ctr-item {
  width: 20%;
}

.form-info {
  width: 100%;
  display: flex;
  justify-content: space-between;
  flex-wrap: wrap;
  row-gap: 14px;
}

.form-group-row + .form-group-row {
  margin-top: 14px;
}

.form-group-row {
  display: flex;
  column-gap: 8px;
}

.form-info-left {
  display: flex;
  flex-direction: column;
  row-gap: 14px;
  width: calc(50% - 16px);
}

.form-info-right {
  display: flex;
  flex-direction: column;
  row-gap: 14px;
  width: calc(50% - 16px);
}

.form-info-bottom {
  width: 100%;
}

.m-row {
  display: flex;
  column-gap: 8px;
}

.m-row > div:first-child {
  width: 40%;
}

.second-row > div:first-child {
  width: 60%;
}

.second-row > div:last-child {
  width: 40%;
}

.form-info-right > .second-row {
  display: flex;
  align-items: flex-start;
  column-gap: 8px;
}

.m-row > div:last-child {
  flex: 1;
}

.form-group .item-gt {
  padding-left: 8px;
}

.item-contact {
  width: 25%;
}

.department {
  width: 100%;
}

.paging__combobox {
  width: 220px;
}

.paging__combobox > .rotate-180 {
  transform: rotate(-180deg);
}

.form-checkbox {
  display: flex;
  align-items: center;
  column-gap: 8px;
  position: absolute;
  top: 28px;
  left: 200px;
}

.popup__title {
  display: flex;
  align-items: center;
  column-gap: 8px;
}

.popup__title .checkbox {
  margin-left: 8px;
}

.popup__title label {
  font-size: 14px;
  font-weight: 500;
}

.table__row:has(.abc) > td {
  z-index: 111111 !important;
}

.searchbox:has(input:focus) {
  border-color: #50b83c;
}

.delete-popup {
  width: 450px;
  box-shadow: 0 0 4px rgba(23, 27, 42, 0.24);
}

.ppd__img {
  width: 100%;
}

.imglist {
  display: flex;
  column-gap: 2%;
  margin-top: 16px;
}
.imglist img {
  width: 15%;
  box-sizing: border-box;
  border: 1px solid var(--border-color);
}

.ppd__info div,
.ppd__info label {
  align-items: center;
  font-size: 18px;
}

.ppd__info label {
  font-weight: 600;
}

.ppd__info p {
  font-size: 16px;
  margin: 0;
}

.ppd__title {
  font-weight: bold !important;
  font-size: 24px !important;
}

.ppd__price {
  color: var(--red-color);
}
</style>