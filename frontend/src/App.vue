<template>
  <div id="app">
    <img alt="Vue logo" src="./assets/logo.png" />
    <div class="search-box">
      <Search v-on:search="search" />
      <ProductList v-bind:products="products" />
    </div>
  </div>
</template>

<script>
import Search from "./components/Search.vue";
import ProductList from "./components/ProductList.vue";

export default {
  name: "App",
  components: {
    Search,
    ProductList,
  },
  data: function () {
    return {
      products: [],
    };
  },
  methods: {
    search: function (query) {
      if (query && query.length > 2) {
        fetch(`/api/indexers/search?type=ascii-folding&query=${query}`)
          .then((response) => response.json())
          .then((data) => (this.products = data));
      } else {
        this.products = [];
      }
    },
  },
};
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}

.search-box {
  width: 200px;
  margin-left: auto;
  margin-right: auto;
  display: flex;
  flex-direction: column;
}
</style>
