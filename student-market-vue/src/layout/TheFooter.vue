<template>
  <footer class="footer">
    <div class="container">
      <div class="row">
        <div class="col-md-6">
          <h5>About Us</h5>
          <p>
            Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do
            eiusmod tempor incididunt ut labore et dolore magna aliqua.
          </p>
        </div>
        <div class="col-md-6">
          <h5>Contact Us</h5>
          <ul>
            <li><i class="fa fa-map-marker"></i> Nguyên Xá, Hà Nội</li>
            <li><i class="fa fa-phone"></i> 0364 568 958</li>
            <li><i class="fa fa-envelope"></i> ngvanhuy153@gmail.com</li>
          </ul>
        </div>
      </div>
    </div>
  </footer>
</template>

<script>
export default {
  name: "TheFooter",
};
</script>

<style scoped>
.footer {
  height: 240px;
  background-color: #f8f9fa;
  padding: 50px 0;
}

.footer h5 {
  font-size: 18px;
  font-weight: bold;
  color: #333333;
  margin-bottom: 20px;
}

.footer p {
  font-size: 14px;
  line-height: 22px;
  color: #777777;
  margin-bottom: 20px;
}

.footer ul {
  list-style: none;
  padding: 0;
  margin: 0;
}

.footer ul li {
  font-size: 14px;
  line-height: 22px;
  color: #777777;
  margin-bottom: 10px;
}

.footer ul li i {
  margin-right: 10px;
  color: #333333;
}
</style>