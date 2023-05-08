<template>
  <div class="the_messages">
    <div class="list_mess col-md-4">
      <div class="the_messages__title">Tin Nhắn</div>
      <router-link
        v-for="(mess, indexMess) in listMess"
        :key="indexMess"
        :to="`/tin-nhan/${getWithUser(mess)}`"
        class="messlink"
        ><div class="messinfo">
          <img :src="url + `Images/users/avatar/${mess.Avatar}`" />
          <div class="mess__content">
            <div>{{ mess.FullName }}</div>
            <div
              v-if="mess.FromUser != user.UserID"
              class="last-mess"
              :class="mess.Seen ? 'last-mess--seen' : 'last-mess--unread'"
            >
              <div>{{ mess.Content }}</div>
              <div>{{ HCommon.formatTime(mess.CreatedDate) }}</div>
            </div>
            <div v-else class="last-mess">
              <div>Bạn: {{ mess.Content }}</div>
              <div>{{ HCommon.formatTime(mess.CreatedDate) }}</div>
            </div>
          </div>
        </div>
      </router-link>
    </div>
    <div class="mess_detail col-md-8">
      <router-view></router-view>
    </div>
  </div>
</template>

<script>
import { getUserFromLocalStorage } from "@/stores/localStorage";
import connection from "@/js/hubs/chatHub";
export default {
  name: "TheMessages",
  created() {
    this.user = getUserFromLocalStorage();
    this.urlMess = this.HConfig.API.Messages + this.user.UserID;
    this.loadMessageList();
  },
  async mounted() {
    if (connection.state === "Disconnected") {
      await connection.start();
    }
    connection.invoke("JoinGroup", this.user.UserID);
    connection.on("ReceiveMessage", () => {
      this.loadMessageList();
    });
  },
  methods: {
    loadMessageList() {
      try {
        this.axios.get(this.urlMess).then((response) => {
          if (response.data.Success) {
            this.listMess = response.data.Data;
            console.log(this.listMess);
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
  },
  beforeUnmount() {
    connection.invoke("LeaveGroup", this.user.UserID);
    connection.stop();
  },
  data() {
    return {
      url: this.HConfig.URL,
      urlMess: null,
      user: {},
      listMess: [],
    };
  },
};
</script>

<style>
.the_messages {
  margin: auto;
  width: 1200px;
  display: flex;
  align-items: stretch;
  background-color: #fff;
}
.list_mess,
.mess_detail {
  height: calc(100vh - 60px);
  overflow: auto;
}
.list_mess {
  padding: 8px;
  border-right: 1px solid var(--border-color);
}

.messlink {
  display: block;
  padding: 16px;
  border-radius: 8px;
  background: #fff;
}

.messlink:hover {
  background-color: #f6f9fa;
}

.messinfo {
  display: flex;
  column-gap: 8px;
  color: var(--text-color);
}

.messinfo img {
  width: 40px;
  height: 40px;
  border-radius: 20px;
  object-fit: cover;
}

.mess__content > div:first-child {
  font-size: 16px;
  font-weight: 600;
}

.last-mess--seen {
  opacity: 0.9;
}

.last-mess--unread {
  font-weight: 600;
}
.last-mess--unread > div:last-child {
  color: var(--blue-color);
}
.the_messages__title {
  padding-left: 8px;
  font-size: 24px;
  font-weight: bold;
}
</style>