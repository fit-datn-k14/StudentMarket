<template>
  <div class="post-list">
    <div v-for="(post, indexPost) in posts" :key="indexPost">
      <h-post :post="post" />
    </div>
  </div>
</template>

<script>
import HPost from "@/components/postlist/HPost.vue";
export default {
  name: "HPostList",
  props: {
    posts: {
      type: Array,
    },
  },
  components: {
    HPost,
  },
  computed: {},
};
</script>

<style scoped>
@import url(@/css/base/postlist.css);
</style>