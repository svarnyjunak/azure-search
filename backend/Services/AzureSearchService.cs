using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MartinBartos.AzureCognitiveSearch.Models;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace MartinBartos.AzureCognitiveSearch.Services
{
    public class AzureSearchService
    {
        private readonly SearchServiceClient client;
        public const string IndexName = "test";
        private const string suggesterName = "nameSuggestere";

        public AzureSearchService(SearchServiceClient client)
        {
            this.client = client;
        }

        public async Task CreateIndexAsync<T>(IList<T> dataToIndex) where T : IProductModel
        {
            // Create index
            var index = new Index
            {
                Name = IndexName,
                Fields = FieldBuilder.BuildForType<T>(),
                Suggesters = new List<Suggester>() {new Suggester()
                {
                    Name = suggesterName,
                    SourceFields = new string[] { "name" }
                }}
            };

            await client.Indexes.CreateAsync(index);

            // Index data.
            var indexClient = client.Indexes.GetClient(IndexName);
            var batch = new IndexBatch<T>(dataToIndex.Select(d => new IndexAction<T>(d)).ToList());
            indexClient.Documents.Index(batch);
        }

        public async Task<IEnumerable<ProductModel>> SearchAsync(string query)
        {
            var indexSearch = client.Indexes.GetClient(IndexName);
            var searchParameters = new SearchParameters
            {
                QueryType = QueryType.Full
            };

            var foundItems = await indexSearch.Documents.SearchAsync<ProductModel>(query, searchParameters);
            return foundItems.Results.Select(d => d.Document);
        }

        public async Task<IEnumerable<ProductModel>> SuggestAsync(string query)
        {
            var options = new SuggestParameters
            {
                UseFuzzyMatching = true,
                Top = 5
            };

            var indexSearch = client.Indexes.GetClient(IndexName);
            var suggestResult = await indexSearch.Documents.SuggestAsync<ProductModel>(query, suggesterName, options);
            return suggestResult.Results.Select(d => d.Document);
        }
    }
}