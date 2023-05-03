<template>
  <div class="the_messages">
    <div class="list_mess col-md-4"></div>
    <div class="mess_detail col-md-8">
      <router-view></router-view>
    </div>
  </div>
</template>

<script>
import { getUserFromLocalStorage } from "@/stores/localStorage";
export default {
  name: "TheMessages",
  created() {
    this.user = getUserFromLocalStorage();
  },
  methods: {},
  data() {
    return {
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
  background-color: #fff;
}
.list_mess,
.mess_detail {
  height: calc(100vh - 80px);
  overflow: auto;
}
.list_mess {
  background-color: gold;
}
.mess_detail {
  background-color: coral;
}
</style>