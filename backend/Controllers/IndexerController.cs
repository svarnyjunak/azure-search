using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MartinBartos.AzureCognitiveSearch.Models;
using MartinBartos.AzureCognitiveSearch.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Search;

namespace MartinBartos.AzureCognitiveSearch.Controllers
{
    [Route("api/indexers")]
    [ApiController]
    public class IndexerController : ControllerBase
    {
        private readonly SearchServiceClient client;
        private static IList<ProductModel> products = CreateProducts().ToList();

        public IndexerController(SearchServiceClient client)
        {
            this.client = client;
        }

        [HttpGet]
        public ActionResult<List<IndexerModel>> Get()
        {
            return new List<IndexerModel>
            {
                new IndexerModel{ Name = "basic", Title = "Basic" },
                new IndexerModel{ Name = "ascii-folding", Title = "Ascii folding" }
            };
        }

        [HttpPost("create-index/{type}")]
        public async Task<ActionResult> PostCreateIndex(string type)
        {
            var indexName = AzureSearchService.IndexName;

            if (await client.Indexes.ExistsAsync(indexName))
            {
                await client.Indexes.DeleteAsync(indexName);
            }

            await GetStrategy(type).CreateIndexAsync(products);

            return Ok();
        }

        [HttpGet("search")]
        public async Task<IEnumerable<ProductModel>> Search(string type, string query)
        {
            return await GetStrategy(type).SearchAsync(query + "*");
        }

        private static IEnumerable<ProductModel> CreateProducts()
        {
            yield return new ProductModel { Name = "Šála" };
            yield return new ProductModel { Name = "Čepice" };
            yield return new ProductModel { Name = "Rukavice" };
            yield return new ProductModel { Name = "Trenýrky" };
            yield return new ProductModel { Name = "Slipy" };
            yield return new ProductModel { Name = "Ponožky" };
            yield return new ProductModel { Name = "Tílko" };
            yield return new ProductModel { Name = "Tričko" };
            yield return new ProductModel { Name = "Mikina" };
            yield return new ProductModel { Name = "Svetr" };
            yield return new ProductModel { Name = "Letní bunda" };
            yield return new ProductModel { Name = "Zimní bunda" };
            yield return new ProductModel { Name = "Lyžařská bunda" };
            yield return new ProductModel { Name = "Nepromokavá bunda" };
            yield return new ProductModel { Name = "Tepláky" };
            yield return new ProductModel { Name = "Volnočasové kalhoty" };
            yield return new ProductModel { Name = "Džíny" };
            yield return new ProductModel { Name = "Kraťasy" };
            yield return new ProductModel { Name = "Šátek" };
            yield return new ProductModel { Name = "Kšiltovka" };
            yield return new ProductModel { Name = "Plavky" };
            yield return new ProductModel { Name = "Sukně" };
            yield return new ProductModel { Name = "Šaty" };
        }

        private IIndexStrategy GetStrategy(string type)
        {
            switch (type)
            {
                case "basic":
                    return new BasicIndexStrategy(client);
                case "ascii-folding":
                    return new AsciiFoldingIndexStrategy(client);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}