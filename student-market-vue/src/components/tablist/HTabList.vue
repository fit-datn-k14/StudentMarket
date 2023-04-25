<template>
  <div class="tablist">
    <div v-for="(tab, index) in listTabs" :key="index">
      <router-link :to="tab.TabRouter">
        <div class="tab-title">
          <p>{{ tab.TabTitle }}</p>
        </div></router-link
      >
    </div>
  </div>
</template>

<script>
export default {
  name: "HTabList",
  props: {
    listTabs: {
      type: Array,
    },
  },
};
</script>

<style>
.tablist {
  display: flex;
  align-items: end;
  height: 40px;
  border-bottom: 1px solid #ccc;
}

.tab-title {
  height: 32px;
  color: #111;
  padding: 8px 16px 12px;
  border-radius: 4px 4px 0 0;
  box-sizing: border-box;
}

.router-link-active > .tab-title {
  background-color: #f2ebbf;
  font-weight: 700;
  box-shadow: 0 0 4px #ccc;
  border-bottom: 4px solid var(--primary-color);
}
</style>