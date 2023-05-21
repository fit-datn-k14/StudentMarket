<template>
  <router-link
    :to="'/post/' + post.PostID"
    class="d-flex align-items-center post"
    :class="displayType"
  >
    <div class="post__left">
      <img
        class="post__img"
        :src="URL + `Images/posts/${post.PostID}/${post.ImageName}`"
      />
    </div>
    <div class="post__right">
      <div class="post__title">
        <strong>{{ post.Title }}</strong>
      </div>
      <div class="post__price">
        <strong v-if="post.Price">{{ HCommon.formatMoney(post.Price) }}</strong>
        <strong v-else>Liên hệ</strong>
      </div>
      <div class="post__describe">
        <p>{{ post.PostDescribe }}</p>
      </div>
      <div class="post__info">
        <span class="poster__info">
          <img
            v-if="post.Avatar"
            :src="URL + `images/users/avatar/` + post.Avatar"
            alt=""
          />
          <img
            v-if="!post.Avatar"
            src="@/assets/img/default-avatar.jpg"
            alt=""
          />
          {{ post.FullName }}</span
        >
        <span class="post__time"
          >&nbsp;{{ HCommon.formatTime(post.CreatedDate) }}&nbsp;</span
        >
        <span class="post__address">{{ post.LocationName }}</span>
      </div>
      <div class="post__heart">
        <i
          v-if="listIdFavouritePosts.includes(post.PostID)"
          class="fa-solid fa-heart"
          @click.prevent="onClickUnFavouritePosts($event)"
        ></i>
        <i
          v-else
          class="fa-regular fa-heart"
          @click.prevent="onClickFavouritePosts($event)"
        ></i>
      </div>
    </div>
  </router-link>
</template>

<script>
import {
  getFavouritePostsFromLocalStorage,
  addFavouritePostIdToLocalStorage,
  removeFavouritePostIdFromLocalStorage,
  getUserFromLocalStorage,
} from "@/stores/localStorage";
export default {
  name: "HPost",
  props: {
    post: {
      type: Object,
    },
    displayType: {
      type: String,
      required: false,
      default: "type-1",
    },
  },
  async created() {
    var user = getUserFromLocalStorage();
    if (user) {
      this.userId = user.UserID;
      await this.getListIdFavouritePosts();
    }
  },
  methods: {
    setDefaultImage(event) {
      event.target.src = this.defaultImage;
    },
    async onClickFavouritePosts(event) {
      event.preventDefault();
      if (this.userId) {
        this.favouritePosts();
      } else {
        this.$router.push("/dang-nhap");
      }
    },
    async onClickUnFavouritePosts(event) {
      event.preventDefault();
      if (this.userId) {
        this.unFavouritePosts();
      } else {
        this.$router.push("/dang-nhap");
      }
    },
    async getListIdFavouritePosts() {
      this.listIdFavouritePosts = await getFavouritePostsFromLocalStorage();
    },
    favouritePosts() {
      var url = this.HConfig.API.FavouritePosts;
      this.axios
        .post(url, {
          UserID: this.userId,
          PostID: this.post.PostID,
        })
        .then((response) => {
          if (response.data.Success) {
            addFavouritePostIdToLocalStorage(this.post.PostID);
            this.getListIdFavouritePosts();
          } else {
            this.errorMessage = response.data.UserMsg;
          }
        })
        .catch(() => {
          this.errorMessage = this.HResource.Message.Exception;
        });
    },
    unFavouritePosts() {
      var url = `https://localhost:9999/api/v1/FavouritePosts?userId=${this.userId}&postId=${this.post.PostID}`;
      this.axios
        .delete(url)
        .then((response) => {
          if (response.data.Success) {
            removeFavouritePostIdFromLocalStorage(this.post.PostID);
            this.getListIdFavouritePosts();
          } else {
            this.errorMessage = response.data.UserMsg;
          }
        })
        .catch(() => {
          this.errorMessage = this.HResource.Message.Exception;
        });
    },
  },
  data() {
    return {
      URL: "https://localhost:9999/api/v1/",
      listIdFavouritePosts: [],
      userId: null,
    };
  },
};
</script>

<style>
</style>