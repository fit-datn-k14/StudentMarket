<template>
  <div class="icon-container" @mouseover="mouseover" @mouseleave="mouseleave">
    <div class="icon" :style="style"></div>
    <div v-if="title" class="tooltip">{{ title }}</div>
  </div>
</template>

<script>
export default {
  name: "HIcon",
  props: {
    position: {
      type: String,
      required: true,
    },
    size: {
      type: String,
      required: false,
      default: null,
    },
    title: {
      type: String,
      required: false,
      default: null,
    },
  },
  created() {
    this.positionList = this.position.trim().split(" ");
    if (this.size) {
      this.sizeList = this.size.trim().split(" ");
    }
    this.formatStyle(false);
  },
  methods: {
    mouseover() {
      if (this.positionList[3]) this.formatStyle(true);
    },
    mouseleave() {
      this.formatStyle(false);
    },
    formatStyle(hover) {
      let pos1 = this.positionList[0],
        pos2 = this.positionList[1],
        w = this.sizeList[0],
        h = this.sizeList[1];
      if (hover) {
        pos1 = this.positionList[this.positionList.length - 2];
        pos2 = this.positionList[this.positionList.length - 1];
        w = this.sizeList[this.sizeList.length - 2];
        h = this.sizeList[this.sizeList.length - 1];
      }
      this.style = `background-position: ${pos1} ${pos2}; width: ${w}px; height: ${h}px;`;
    },
  },
  data() {
    return {
      style: "",
      positionList: [],
      sizeList: ["24", "24"],
    };
  },
};
</script>

<style scoped>
.icon-container {
  min-width: 24px;
  min-height: 24px;
  display: flex;
  justify-content: center;
  align-items: center;
  cursor: pointer;
  position: relative;
}
.icon {
  background: url(@/assets/img/Sprites.64af8f61.svg) no-repeat;
}

.icon-container:has(.icon:hover) .tooltip {
  display: block;
}
</style>