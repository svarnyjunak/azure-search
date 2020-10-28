<template>
  <input type="text" v-model="query" v-on:keyup="search" />
</template>

<script>
export default {
  name: "Search",
  data: function () {
    return { query: "" };
  },
  methods: {
    search: function (e) {
      e.preventDefault();
      this.$emit("search", this.query);
    },
  },
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>
