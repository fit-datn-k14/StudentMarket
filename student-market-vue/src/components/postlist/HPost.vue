<template>
  <router-link
    :to="'/post/' + post.PostCode"
    class="d-flex align-items-center post"
    :class="displayType"
  >
    <div class="post__left">
      <img class="post__img" src="../../assets/img/SV_Logo.jpg" />
      <div>{{ post.NumberOfPhotos }} ảnh</div>
    </div>
    <div class="post__right">
      <div class="post__title">
        <strong>{{ post.Title }}</strong>
      </div>
      <div class="post__price">
        <strong v-if="post.Price">{{ post.Price }}</strong>
        <strong v-else>Liên hệ</strong>
      </div>
      <div class="post__describe">
        <p>{{ post.Describe }}</p>
      </div>
      <div class="post__info">
        <span class="poster__info">
          <img v-if="post.avatar" :src="post.avatar" alt="" />
          <img
            v-if="!post.avatar"
            src="@/assets/img/default-avatar.jpg"
            alt=""
          />
          {{ post.FullName }}</span
        >
        <span class="post__time"
          >&nbsp;{{ HCommon.postTime(post.CreatedDate) }}&nbsp;</span
        >
        <span class="post__address">{{ post.LocationName }}</span>
      </div>
      <div class="post__heart">
        <i v-if="post.favourite" class="fa-solid fa-heart"></i>
        <i v-else class="fa-regular fa-heart"></i>
      </div>
    </div>
  </router-link>
</template>

<script>
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
};
</script>

<style>
</style>