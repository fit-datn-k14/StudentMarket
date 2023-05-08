<template>
  <div class="mess_detail">
    <div class="mdv__container" @focus="seenMess">
      <div class="mdv__header shadow-sm">
        <div class="mdv__userinfo">
          <img :src="url + `Images/users/avatar/${withUser.Avatar}`" />
          <span>{{ withUser.FullName }}</span>
        </div>
        <h-icon position="-1338px -409px" size="3 15" />
      </div>
      <div
        class="mdv__content"
        ref="listMess"
        :style="{ height: contentHeight }"
      >
        <div class="listMess">
          <div v-for="(mess, index) in listMessages" :key="index">
            <img
              v-if="
                mess.FromUser === id &&
                (!listMessages[index + 1] ||
                  listMessages[index + 1].FromUser != id)
              "
              width="40"
              height="40"
              style="border-radius: 20px"
              src="@/assets/img/default-avatar.jpg"
              alt=""
            />
            <div v-if="mess.FromUser === id" class="mess__left">
              {{ mess.Content }}
            </div>
            <div v-else class="mess__right">
              {{ mess.Content }}
            </div>
          </div>
        </div>
      </div>
      <div class="addMessage mdv__footer">
        <h-input
          ref="txtNewMessage"
          v-model="newMessage.Content"
          @keyup.enter="addMessage"
        />
        <div class="btn-send" @click="addMessage">
          <i class="fa-regular fa-paper-plane"></i>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { getUserFromLocalStorage } from "@/stores/localStorage";
import HIcon from "@/components/icon/HIcon.vue";
import connection from "@/js/hubs/chatHub";
export default {
  components: { HIcon },
  name: "MessageDetail",
  props: {
    id: {
      type: String,
      required: true,
    },
  },
  async created() {
    this.user = await getUserFromLocalStorage();
    await this.loadWithUser();
    this.newMessage.FromUser = this.user.UserID;
    this.newMessage.ToUser = this.id;
    this.urlMess = this.HConfig.API.Messages + "ListMess";
    this.messDataModel.UserID = this.user.UserID;
    this.messDataModel.WithUser = this.id;
    await this.loadMess();
    await this.seenMess();
  },
  async mounted() {
    // Sau khi component được mounted, tính chiều cao của .mdv__content
    this.contentHeight =
      window.innerHeight - this.$refs.listMess.getBoundingClientRect().top;
    this.$refs.listMess.scrollTo(0, this.contentHeight);
    if (connection.state === "Disconnected") {
      await connection.start();
    }
    connection.invoke("JoinGroup", this.user.UserID);
    connection.on("ReceiveMessage", (mess) => {
      this.listMessages.unshift(this.HCommon.convertCamelToPascal(mess));
    });
  },
  methods: {
    async seenMess() {
      console.log(this.listMessages);
      if (
        this.listMessages.length > 0 &&
        this.listMessages[0].Seen == this.HEnum.seen.Unread &&
        this.listMessages[0].ToUser == this.user.UserID
      ) {
        var urlMess =
          this.HConfig.API.Messages + this.user.UserID + "/" + this.id;
        await this.axios.put(urlMess).catch(() => {
          this.errorMessage = this.HResource.Message.Exception;
        });
      }
    },
    loadWithUser() {
      var url = this.HConfig.API.Users + this.id;
      try {
        this.axios.get(url).then((response) => {
          if (response.data.Success) {
            this.withUser = response.data.Data;
          } else {
            this.errorMessage = response.data.UserMsg;
          }
        });
      } catch {
        this.errorMessage = this.HResource.Text.MessageException;
      }
    },
    async loadMess() {
      try {
        await this.axios
          .post(this.urlMess, this.messDataModel)
          .then((response) => {
            if (response.data.Success) {
              this.listMessages = response.data.Data;
            } else {
              this.errorMessage = response.data.UserMsg;
            }
          });
      } catch {
        this.errorMessage = this.HResource.Text.MessageException;
      }
    },
    addMessage() {
      if (this.newMessage.Content) {
        var url = this.HConfig.API.Messages;
        this.axios
          .post(url, this.newMessage)
          .then((response) => {
            if (response.data.Success) {
              this.newMessage.Content = null;
              this.getMessages();
            }
          })
          .catch((error) => {
            this.errorMessage = error;
          });
      }
    },
  },
  data() {
    return {
      url: this.HConfig.URL,
      messDataModel: {},
      urlMess: {},
      user: {},
      withUser: {},
      newMessage: {},
      listMessages: [],
      contentHeight: 0,
    };
  },
};
</script>

<style scoped>
@import url(@/css/views/messagedetail.css);
</style>