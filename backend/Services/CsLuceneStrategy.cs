using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MartinBartos.AzureCognitiveSearch.Models;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace MartinBartos.AzureCognitiveSearch.Services
{
    public class CsLuceneStrategy : IIndexStrategy
    {
        private readonly AzureSearchService azureSearch;

        public CsLuceneStrategy(SearchServiceClient client)
        {
            azureSearch = new AzureSearchService(client);
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

        [SerializePropertyNamesAsCamelCase]
        class ProductIndexModel : IProductModel
        {
            [Key]
            [IsFilterable, IsSortable]
            public string Id { get; set; }

            [IsFilterable, IsSortable, IsSearchable]
            [Analyzer(AnalyzerName.AsString.CsLucene)]
            public string Name { get; set; }
        }
    }
}