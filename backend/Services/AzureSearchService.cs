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
        private readonly string indexName = "test";

        public AzureSearchService(SearchServiceClient client)
        {
            this.client = client;
        }

        public async Task CreateIndexAsync<T>(IList<T> dataToIndex)
        {
            // Create index
            var index = new Index
            {
                Name = indexName,
                Fields = FieldBuilder.BuildForType<T>()
            };

            await client.Indexes.CreateAsync(index);

            // Index data.
            var indexClient = client.Indexes.GetClient(indexName);
            var batch = new IndexBatch<T>(dataToIndex.Select(d => new IndexAction<T>(d)).ToList());
            indexClient.Documents.Index(batch);
        }
    }
}