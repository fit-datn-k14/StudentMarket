<template>
  <img class="loading" src="@/assets/img/loading.svg" alt="" />
</template>

<script>
export default {
  name: "HLoading",
};
</script>

<style scoped>
.loading {
  width: 50px;
  height: 50px;
  z-index: 111;
  position: absolute;
  top: 40%;
  left: 45%;
  animation: spin 1s steps(8) infinite;
}

@keyframes spin {
  100% {
    transform: rotate(360deg);
  }
}
</style>