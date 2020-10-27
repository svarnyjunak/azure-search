using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MartinBartos.AzureCognitiveSearch.Models;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace MartinBartos.AzureCognitiveSearch.Services
{
    public class BasicIndexStrategy : IIndexStrategy
    {
        private readonly AzureSearchService azureSearch;

        public BasicIndexStrategy(SearchServiceClient client)
        {
            azureSearch = new AzureSearchService(client);
        }

        public async Task CreateIndexAsync(string indexName, IList<ProductModel> data)
        {
            var dataToIndex = data
                .Select(d => new ProductIndexModel
                {
                    Id = d.Id,
                    Name = d.Name
                })
                .ToList();

            await azureSearch.CreateIndexAsync(dataToIndex);
        }

        [SerializePropertyNamesAsCamelCase]
        class ProductIndexModel
        {
            [Key]
            [IsFilterable, IsSortable]
            public string Id { get; set; }

            [IsFilterable, IsSortable, IsSearchable]
            public string Name { get; set; }
        }
    }
}