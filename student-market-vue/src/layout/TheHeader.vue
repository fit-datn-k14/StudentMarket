<template>
  <div class="header-box shadow-sm">
    <header class="w-100">
      <div
        class="w-100 nav-box d-flex justify-content-between align-items-center"
      >
        <div class="menu-with-logo d-flex align-items-center">
          <div class="logo ms-3">
            <router-link class="header__logo" to="/">
              <img src="@/assets/img/Logo.jpg" alt="" class="logo-img" />
              <strong style="white-space: nowrap; font-size: 20px"
                >Chợ Sinh Viên</strong
              >
            </router-link>
          </div>
          <ul class="menu d-flex">
            <li v-for="(tab, indexTab) in headerList" :key="indexTab">
              <router-link :to="tab.link" class="link">
                <div class="text">{{ tab.title }}</div>
                <div class="arrow"></div>
              </router-link>
            </li>
            <li v-if="censor">
              <router-link to="/admin" class="link">
                <div class="text">Admin</div>
                <div class="arrow"></div>
              </router-link>
            </li>
          </ul>
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
        <div class="login-group me-4 d-flex align-items-center">
          <div class="all-btn d-flex align-items-center">
            <router-link
              to="/tin-nhan"
              class="favorite-post-link"
              title="Tin nhắn"
            >
              <i class="header-icon fa-sharp fa-solid fa-comment"></i>
              <div v-if="numberChatUnread" class="number_unread_chat">
                {{ numberChatUnread }}
              </div>
            </router-link>
            <router-link
              to="/tin-dang-yeu-thich"
              class="favorite-post-link"
              title="Danh sách tin yêu thích"
            >
              <i class="header-icon d-block fa-regular fa-heart"></i>
            </router-link>

            <div
              class="theheader__notifications"
              title="Thông báo"
              v-click-outside="hiddenNotifications"
            >
              <i
                class="bell-icon fa-solid fa-bell header-icon"
                @click="showNotifications = !showNotifications"
              ></i>
              <div v-if="numberNotifyUnread" class="number_unread_notify">
                {{ numberNotifyUnread }}
              </div>
              <div class="notificationbox shadow" v-show="showNotifications">
                <div class="notificationbox__title">Thông báo</div>
                <div class="notificationbox__options">
                  <h-button
                    class="btn-option"
                    :type="
                      HEnum.optionNotify.All == selectedOptionNotify
                        ? 'btn-pri'
                        : 'btn-second'
                    "
                    value="Tất cả"
                    @click="onSelectOptionNotify(HEnum.optionNotify.All)"
                  />
                  <h-button
                    class="btn-option"
                    :type="
                      HEnum.optionNotify.Unread == selectedOptionNotify
                        ? 'btn-pri'
                        : 'btn-second'
                    "
                    value="Chưa đọc"
                    @click="onSelectOptionNotify(HEnum.optionNotify.Unread)"
                  />
                </div>
                <div class="notificationbox__list">
                  <div
                    class="notificationlist"
                    v-if="notificationFilter.length > 0"
                  >
                    <div
                      class="notify_item"
                      v-for="(notify, notifyIndex) in notificationFilter"
                      :class="
                        notify.Seen
                          ? 'notify_item--seen'
                          : 'notify_item--unread'
                      "
                      :key="notifyIndex"
                      @click="onClickNotify(notify)"
                    >
                      <img
                        class="notify__avatar"
                        :src="URL + `Images/users/avatar/${notify.Avatar}`"
                      />
                      <div>
                        <div v-html="HCommon.formatNotify(notify)"></div>
                        <div class="notify_time">
                          {{ HCommon.formatTime(notify.CreatedDate) }}
                        </div>
                      </div>
                    </div>
                  </div>
                  <div class="no-notification" v-else>
                    <h3>{{ notifyMessage }}</h3>
                  </div>
                </div>
              </div>
            </div>

            <router-link to="/dang-tin">
              <HButton
                class="circle"
                ref="btnDangTin"
                value="Đăng Tin"
                type="btn-pri"
                :disabled="false"
              />
            </router-link>
            <router-link v-if="!user" to="/dang-nhap" class="link">
              <div class="text">Đăng nhập</div>
              <div class="arrow"></div>
            </router-link>
            <span v-if="!user" class="line"></span>
            <router-link v-if="!user" to="/dang-ky" class="link">
              <div class="text">Đăng ký</div>
              <div class="arrow"></div>
            </router-link>
            <div
              v-else
              class="header__userinfo"
              @click="showDropdown = !showDropdown"
              v-click-outside="() => (showDropdown = false)"
            >
              <img
                class="theheader__avatar"
                :src="URL + `Images/users/avatar/${user.Avatar}`"
              />
              <span>{{ user.FullName }}</span>
              <div v-show="showDropdown" class="header__dropdown">
                <router-link
                  to="/cai-dat-tai-khoan"
                  class="link"
                  @click="showDropdown = false"
                >
                  <div class="text">Cài Đặt Tài Khoản</div>
                </router-link>
                <router-link
                  to="/doi-mat-khau"
                  class="link"
                  @click="showDropdown = false"
                >
                  <div class="text">Đổi mật khẩu</div>
                </router-link>
                <HButton type="btn-link" value="Đăng Xuất" @click="logout" />
              </div>
            </div>
          </div>
        </div>
      </div>
    </header>
  </div>
</template>
  
  <script>
import { isCensor } from "../stores/utils.js";
import { eventBus } from "@/js/eventbus";
import {
  getUserFromLocalStorage,
  removeUserFromLocalStorage,
  saveFavouritePostsToLocalStorage,
  removeAllFavouritesFromLocalStorage,
} from "@/stores/localStorage.js";
import connection from "@/js/hubs/notificationHub";
import connectionChat from "@/js/hubs/chatHub";
import HConfig from "@/js/base/config.js";

export default {
  name: "TheHeader",
  async created() {
    await this.setUser();
  },
  methods: {
    loadMessageList() {
      try {
        var url = this.HConfig.API.Messages + this.user.UserID;
        this.axios.get(url).then((response) => {
          if (response.data.Success) {
            var list_mess = response.data.Data;
            this.numberChatUnread = list_mess.filter(
              (n) =>
                n.Seen == this.HEnum.seen.Unread && n.ToUser == this.user.UserID
            ).length;
          } else {
            this.errorMessage = response.data.UserMsg;
          }
        });
      } catch {
        this.errorMessage = this.HResource.Text.MessageException;
      }
    },
    getWithUser(mess) {
      return mess.FromUser == this.user.UserID ? mess.ToUser : mess.FromUser;
    },
    onSearch() {
      eventBus.emit("search", this.txtSearch);
      this.$router.push("/trang-chu");
      this.txtSearch = null;
    },
    logout() {
      connection.invoke("LeaveGroup", this.user.UserID);
      connectionChat.invoke("LeaveGroup", this.user.UserID);
      removeUserFromLocalStorage();
      removeAllFavouritesFromLocalStorage();
      this.showDropdown = false;
      this.setUser();
      this.numberChatUnread = 0;
      this.$router.push("/");
    },
    onSelectOptionNotify(value) {
      this.selectedOptionNotify = value;
    },
    async setUser() {
      this.user = await getUserFromLocalStorage();
      this.censor = isCensor();
      if (this.user) {
        await this.getFavouritePosts();
      }
      if (this.user) {
        await this.getNotifications();
        this.loadMessageList();
        this.notifyMessage = "Không có thông báo nào";
      } else {
        this.notificationList = [];
        this.notifyMessage = "Bạn phải đăng nhập để xem thông báo";
      }
    },
    async onClickNotify(notify) {
      await this.seenNotify(notify);
      notify.Seen = 1;
    },
    async seenNotify(notify) {
      var urlNotify = this.HConfig.API.Notifications + notify.NotificationID;
      await this.axios.put(urlNotify).catch(() => {
        this.errorMessage = this.HResource.Message.Exception;
      });
      if (notify.PostID) {
        this.$router.push(`/post/${notify.PostID}`);
      }
      this.showNotifications = false;
    },
    async getFavouritePosts() {
      var url =
        this.HConfig.API.FavouritePosts + "GetListIds/" + this.user.UserID;
      await this.axios
        .get(url)
        .then((response) => {
          if (response.data.Success) {
            saveFavouritePostsToLocalStorage(response.data.Data);
          }
        })
        .catch(() => {
          this.errorMessage = this.HResource.Message.Exception;
        });
    },
    async getNotifications() {
      var urlNotify = this.HConfig.API.Notifications + this.user.UserID;
      await this.axios
        .get(urlNotify)
        .then((response) => {
          if (response.data.Success) {
            this.notificationList = response.data.Data;
          }
        })
        .catch(() => {
          this.errorMessage = this.HResource.Message.Exception;
        });
    },
    hiddenNotifications() {
      this.showNotifications = false;
    },
  },
  computed: {
    numberNotifyUnread() {
      var result = this.notificationList.filter(
        (n) => n.Seen == this.HEnum.seen.Unread
      ).length;
      return result;
    },
    notificationFilter() {
      if (this.selectedOptionNotify == this.HEnum.optionNotify.Unread) {
        var result = this.notificationList.filter(
          (n) => n.Seen == this.HEnum.seen.Unread
        );
        return result;
      }
      return this.notificationList;
    },
  },
  async mounted() {
    if (connection.state === "Disconnected") {
      connection.start();
    }
    if (connectionChat.state === "Disconnected") {
      await connectionChat.start();
    }
    eventBus.on("login", () => {
      this.setUser();
      if (connection.state === "Disconnected") {
        connection.start().then(() => {
          connection.invoke("JoinGroup", this.user.UserID);
        });
      }
      if (connectionChat.state === "Disconnected") {
        connectionChat.start().then(() => {
          connectionChat.invoke("JoinGroup", this.user.UserID);
        });
      }
      this.loadMessageList();
    });
    if (this.user) {
      connection.invoke("JoinGroup", this.user.UserID);
      connectionChat.invoke("JoinGroup", this.user.UserID);
    }
    connection.on("ReceiveNotification", (notification) => {
      this.notificationList.unshift(
        this.HCommon.convertCamelToPascal(notification)
      );
    });
    connectionChat.on("ReceiveMessage", () => {
      this.loadMessageList();
    });
    eventBus.on("updateUser", () => {
      this.user = getUserFromLocalStorage();
    });
  },
  beforeUnmount() {
    connection.invoke("LeaveGroup", this.user.UserID);
    connection.stop();
  },
  data() {
    return {
      URL: HConfig.URL,
      showDropdown: false,
      selectedOptionNotify: 0,
      showNotifications: false,
      numberChatUnread: 0,
      txtSearch: "",
      censor: false,
      user: {},
      userInfo: {},
      notificationList: [],
      notifyMessage: "",
      headerList: [
        {
          link: "/trang-chu",
          icon: "",
          title: "Trang Chủ",
        },
        {
          link: "/quan-ly-tin-dang",
          icon: "",
          title: "Tin Của Tôi",
        },
      ],
    };
  },
};
</script>
  
<style >
@import url(@/css/layout/theheader.css);
</style>
  