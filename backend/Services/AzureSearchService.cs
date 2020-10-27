using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace MartinBartos.AzureCognitiveSearch.Services
{
    public class AzureSearchService
    {
        private readonly SearchServiceClient client;
        public const string IndexName = "test";

        public AzureSearchService(SearchServiceClient client)
        {
            this.client = client;
        }

        public async Task CreateIndexAsync<T>(IList<T> dataToIndex)
        {
            // Create index
            var index = new Index
            {
                Name = IndexName,
                Fields = FieldBuilder.BuildForType<T>()
            };

            await client.Indexes.CreateAsync(index);

            // Index data.
            var indexClient = client.Indexes.GetClient(IndexName);
            var batch = new IndexBatch<T>(dataToIndex.Select(d => new IndexAction<T>(d)).ToList());
            indexClient.Documents.Index(batch);
        }

        public async Task<IEnumerable<T>> SearchAsync<T>(string query)
        {
            var indexSearch = client.Indexes.GetClient(IndexName);
            var foundItems = await indexSearch.Documents.SearchAsync<T>(query);
            return foundItems.Results.Select(d => d.Document);
        }
    }
}