<template>
  <div class="single-product mt-150 mb-150">
    <div class="container">
      <div class="row">
        <div class="col-md-4">
          <div class="single-product-img">
            <img
              class="udv_avatar border border-dark"
              :src="URL + `Images/users/avatar/${User.Avatar}`"
            />
          </div>
        </div>
        <div class="col-md-4">
          <div class="single-product-content">
            <h3>{{ User.FullName }}</h3>

            <p class="">
              <strong>Vai Trò:&nbsp;</strong>
              {{ HCommon.formatRole(User.Role) }}
            </p>
            <p v-if="User.DateOfBirth" class="">
              <strong>Ngày Sinh:&nbsp;</strong>
              {{ HCommon.formatDate(User.DateOfBirth, 1) }}
            </p>
            <p v-else class=""><strong>Ngày Sinh:&nbsp;</strong>Chưa có</p>
            <p v-if="User.Gender != null" class="">
              <strong>Giới Tính:&nbsp;</strong>
              {{ HCommon.formatGender(User.Gender) }}
            </p>
            <p v-else class=""><strong>Giới Tính:&nbsp;</strong>Chưa có</p>
            <p v-if="User.LocationName" class="">
              <strong>Khu vực:&nbsp;</strong>
              {{ User.LocationName }}
            </p>
            <p v-else class=""><strong>Khu vực:&nbsp;</strong>Chưa có</p>
            <strong>Địa Chỉ:&nbsp;</strong>
            <p v-if="User.Address" class="">
              {{ User.Address }}
            </p>
            <p v-else class="">Chưa có</p>
          </div>
        </div>
        <div class="col-md-4">
          <div class="single-product-content">
            <p><strong>Thông Tin Liên Hệ</strong></p>
            <p v-if="User.PhoneNumber" class="">
              <strong>Số Điện Thoại:&nbsp;</strong>
              {{ User.PhoneNumber }}
            </p>
            <p v-else class=""><strong>Số Điện Thoại:&nbsp;</strong>Chưa có</p>
            <p v-if="User.Email" class="">
              <strong>Email:&nbsp;</strong>
              {{ User.Email }}
            </p>
            <p v-else class=""><strong>Email:&nbsp;</strong>Chưa có</p>
            <strong>Số CCCD/CMND:&nbsp;</strong>
            <p v-if="User.IdentityNumber" class="">
              {{ User.IdentityNumber }}
            </p>
            <p v-else class="">Chưa có</p>
            <router-link to="/thay-doi-thong-tin-ca-nhan">
              <HButton
                class="circle"
                ref="btnChangeInfo"
                value="Thay Đổi Thông Tin Cá Nhân"
                type="btn-pri"
              />
            </router-link>
          </div>
        </div>
      </div>
    </div>
  </div>
  <h-loading v-if="isLoading"></h-loading>
</template>
    <script>
import HConfig from "@/js/base/config";
import { getUserFromLocalStorage } from "@/stores/localStorage.js";
export default {
  name: "UserDetail",
  props: {
    id: {
      type: String,
      required: true,
    },
  },
  async created() {
    this.User = await getUserFromLocalStorage();
    this.ImageID = this.Post.ImageName;
  },
  methods: {},

  data() {
    return {
      URL: HConfig.URL,
      User: {},
      Post: {},
      Seller: {},
      Comments: [],
      newComment: {},
      ImageID: null,
    };
  },
};
</script>
  
  <style scoped>
.udv_avatar {
  width: 100%;
  border-radius: 50%;
  aspect-ratio: 1 / 1;
  -o-object-fit: cover;
  object-fit: cover;
}

.ppd__img {
  width: 100%;
}

.single-product-describe {
  min-height: 180px;
}

.single-product {
  padding: 70px 0;
}

.single-product > div {
  background-color: #fff;
  padding: 24px;
  border-radius: 4px;
  margin-bottom: 12px;
}

.single-product-img {
  width: 100%;
}

.single-product-content {
  padding-left: 40px;
}

.single-product-content h3 {
  font-size: 36px;
  font-weight: 700;
  margin-bottom: 20px;
}

.single-product-pricing {
  font-size: 24px;
  font-weight: 600;
  margin-bottom: 30px;
  color: var(--red-color);
}

.single-product-pricing span {
  font-size: 18px;
  margin-right: 10px;
}

.single-product-content p {
  font-size: 18px;
  line-height: 28px;
  margin-bottom: 30px;
}

.btn-mess {
  background-color: var(--primary-color);
  width: 96px;
  height: 32px;
  display: flex;
  justify-content: center;
  align-items: center;
  border-radius: 16px;
}

.btn-mess a {
  color: #fff;
}

.cart-btn {
  display: inline-block;
  padding: 10px 30px;
  font-size: 18px;
  font-weight: 700;
  background-color: #000;
  color: #fff;
  border-radius: 30px;
  transition: all 0.3s ease;
}

.cart-btn:hover {
  transform: translateY(-3px);
  box-shadow: 0 5px 15px rgba(0, 0, 0, 0.3);
}

.single-product-content strong {
  font-weight: 700;
  font-size: 18px;
}

.single-product-categories {
}

.product-share {
  display: flex;
  align-items: center;
  margin-top: 40px;
}

body {
  font-family: "Open Sans", sans-serif;
  font-weight: 400;
  font-size: 1rem;
  letter-spacing: 0.1px;
  line-height: 1.8;
  color: #051922;
  overflow-x: hidden;
}
</style>