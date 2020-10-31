using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MartinBartos.AzureCognitiveSearch.Models;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace MartinBartos.AzureCognitiveSearch.Services
{
    public class StandardLuceneStrategy : IIndexStrategy
    {
        private readonly AzureSearchService azureSearch;
        private readonly SearchServiceClient client;

        public StandardLuceneStrategy(SearchServiceClient client)
        {
            azureSearch = new AzureSearchService(client);
            this.client = client;
        }

        public async Task CreateIndexAsync(IList<ProductModel> data)
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

        public async Task<IEnumerable<ProductModel>> SearchAsync(string query)
        {
            return await azureSearch.SearchAsync<ProductModel>(query);
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