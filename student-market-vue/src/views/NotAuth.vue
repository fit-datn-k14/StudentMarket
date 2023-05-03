<template>
  <div class="error-container">
    <h1>401 - Unauthenticated</h1>
    <p>Bạn không có quyền sử dụng chức năng này</p>
  </div>
</template>
  
  <script>
</script>
  
    <style scoped>
.error-container {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  width: 100%;
  height: calc(100vh - 80px);
  background-color: #eeeff1;
}

h1 {
  font-size: 6em;
  margin-bottom: 0;
}

p {
  font-size: 2em;
  margin-top: 0;
}
</style>