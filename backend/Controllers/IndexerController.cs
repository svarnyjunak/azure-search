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
            var indexName = "test";

            if (await client.Indexes.ExistsAsync(indexName))
            {
                await client.Indexes.DeleteAsync(indexName);
            }

            await GetStrategy(type).CreateIndexAsync(indexName, products);

            return Ok();
        }

        private static IEnumerable<ProductModel> CreateProducts()
        {
            yield return new ProductModel { Name = "Šála" };
            yield return new ProductModel { Name = "Čepice" };
            yield return new ProductModel { Name = "Rukavice" };
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