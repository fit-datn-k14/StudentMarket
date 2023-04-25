<template>
  <div class="single-product mt-150 mb-150">
    <div class="container">
      <div class="row">
        <div class="col-md-5">
          <div class="single-product-img">
            <img src="../../assets/img/SV_Logo.jpg" alt="" />
          </div>
        </div>
        <div class="col-md-7">
          <div class="single-product-content">
            <h3>{{ title }}</h3>
            <p class="single-product-pricing">
              <span>Per Kg</span> {{ price }}
            </p>
            <p>{{ description }}</p>
            <div class="single-product-form">
              <form>
                <input type="number" placeholder="0" v-model="quantity" />
              </form>
              <button class="cart-btn" @click="addToCart">
                <i class="fas fa-shopping-cart"></i> Add to Cart
              </button>
            </div>
            <div><strong>Categories: </strong>{{ categories }}</div>
            <h4>Share:</h4>
            <ul class="product-share">
              <li>
                <a href=""><i class="fab fa-facebook-f"></i></a>
              </li>
              <li>
                <a href=""><i class="fab fa-twitter"></i></a>
              </li>
              <li>
                <a href=""><i class="fab fa-google-plus-g"></i></a>
              </li>
              <li>
                <a href=""><i class="fab fa-linkedin"></i></a>
              </li>
            </ul>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
  <script>
export default {
  name: "ThePostDetail",
  data() {
    return {
      title: "Green apples have polyphenols",
      image: "@/assets/img/SV_Logo.jpg",
      price: 50,
      description:
        "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Dicta sint dignissimos, rem commodi cum voluptatem quae reprehenderit repudiandae ea tempora incidunt ipsa, quisquam animi perferendis eos eum modi! Tempora, earum.",
      categories: "Fruits, Organic",
      quantity: 0,
    };
  },
  methods: {
    addToCart() {
      // Implement addToCart functionality here
    },
  },
};
</script>

<style scoped>
.single-product {
  padding: 70px 0;
}

.single-product-img img {
  width: 100%;
}

.single-product-content {
  padding-left: 40px;
}

.single-product-content h3 {
  font-size: 36px;
  font-weight: 700;
  margin-bottom: 20px;
}

.single-product-pricing {
  font-size: 24px;
  font-weight: 600;
  margin-bottom: 30px;
}

.single-product-pricing span {
  font-size: 18px;
  font-weight: 400;
  margin-right: 10px;
}

.single-product-content p {
  font-size: 18px;
  font-weight: 400;
  line-height: 28px;
  margin-bottom: 30px;
}

.single-product-form {
  display: flex;
  align-items: center;
  margin-bottom: 30px;
}

.single-product-form input[type="number"] {
  width: 70px;
  height: 40px;
  font-size: 18px;
  font-weight: 400;
  border: 1px solid #ddd;
  border-radius: 3px;
  text-align: center;
  margin-right: 20px;
}

.cart-btn {
  display: inline-block;
  padding: 10px 30px;
  font-size: 18px;
  font-weight: 700;
  background-color: #000;
  color: #fff;
  border-radius: 30px;
  transition: all 0.3s ease;
}

.cart-btn:hover {
  transform: translateY(-3px);
  box-shadow: 0 5px 15px rgba(0, 0, 0, 0.3);
}

.single-product-content strong {
  font-weight: 700;
}

.product-share {
  display: flex;
  align-items: center;
  margin-top: 40px;
}

.product-share li {
  margin-right: 20px;
}

.product-share a {
  display: inline-block;
  width: 40px;
  height: 40px;
  line-height: 40px;
  text-align: center;
  background-color: #eee;
  color: #333;
  border-radius: 50%;
  transition: all 0.3s ease;
}

.product-share a:hover {
  background-color: #000;
  color: #fff;
}

body {
  font-family: "Open Sans", sans-serif;
  font-weight: 400;
  font-size: 1rem;
  letter-spacing: 0.1px;
  line-height: 1.8;
  color: #051922;
  overflow-x: hidden;
}
</style>