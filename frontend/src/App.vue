<template>
  <div id="app">
    <h1>Vyhledávání pomocí Azure Cognitive Search</h1>
    <div class="main">
      <div class="article">
        <p>
          Nejdříve je potřeba oblečení zaindexovat, to lze provést pomocí různých
          analyzérů:

        <h2>StandardLucene</h2>
        <p>
          Analyzer můžete na property přidat pomocí atributu <pre class="code--inline">AnalyzerAttribute</pre>, 
          pokud žádný neuvedete, automaticky se použíje <pre class="code--inline">StandardLucene</pre> analyzer. 
          Model indexu pak bude vypadat takto:
      
          <pre><code lang="#">[SerializePropertyNamesAsCamelCase]
class ProductIndexModel
{
    [Key]
    [IsFilterable, IsSortable]
    public string Id { get; set; }

    [IsFilterable, IsSortable, IsSearchable]
    public string Name { get; set; }
}</code></pre>

          <a href="#" class="pure-button pure-button-primary" v-on:click="indexData($event, 'standard-lucene')">Použít StandardLucene</a>
        </p>

        <h2>StandardAsciiFoldingLucene</h2>
        <p>
          Pokud chcete vyhledávat bez diakritiky můžete použít <pre class="code--inline">StandardAsciiFoldingLucene</pre> analyzer:

          <pre><code lang="C#">[SerializePropertyNamesAsCamelCase]
class ProductIndexModel
{
    [Key]
    [IsFilterable, IsSortable]
    public string Id { get; set; }
    
    [IsFilterable, IsSortable, IsSearchable]
    <span class="code--higlighted">[Analyzer(AnalyzerName.AsString.StandardAsciiFoldingLucene)]</span>
    public string Name { get; set; }
}</code></pre>

        <a
            href="#"
            class="pure-button pure-button-primary"
            v-on:click="indexData($event, 'standard-ascii-folding-lucene')"
            >Použít StandardAsciiFoldingLucene</a
          >
        </p>
      </div>
      <div class="search-box">
        <div>Vyhledejte oblečení (např. tričko)</div>
        <Search v-on:search="search" />
        <ProductList v-bind:products="products" />
      </div>
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
    indexData: function (e, analyzerType) {
      e.preventDefault();

      fetch(`/api/indexers/create-index/${analyzerType}`, {
        method: "POST",
      });
    },
    search: function (query) {
      if (query && query.length > 2) {
        fetch(`/api/indexers/search?type=standard-lucene&query=${query}`)
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
html{
  overflow: hidden;
}

#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  color: #2c3e50;
  margin: 20px;
}

.article {
  padding: 1em;
  overflow-y: auto;
  height: calc(100% - 12em);
  border: 1px solid rgb(235, 233, 233);
}

pre {
  padding: 1em;
  background: rgb(235, 233, 233);
}

.code--inline {
  display: inline;
  padding: 0;
  margin: 0;
}

.main {
  display: grid;
  grid-template-columns: 50% 50%;
}

.search-box {
  width: 300px;
  margin-left: auto;
  margin-right: auto;
  display: flex;
  flex-direction: column;
}

.code--higlighted {
  background: yellow;
}
</style>
