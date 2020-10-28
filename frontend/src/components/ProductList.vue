<template>
  <div id="product-list" v-if="products.length" class="product-list">
    <div v-for="product in products" :key="product.id" class="product-list__item">
      {{ product.name }}
    </div>
  </div>
</template>

<script>
export default {
  name: "ProductList",
  props: { products: Array },
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
.product-list{
  border: 1px solid black;  
}

.product-list__item {
  border-top: 1px solid black;
  margin: 3px;
}

.product-list__item:first-child {
    border-top: none;
}
</style>
