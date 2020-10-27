using System.Collections.Generic;
using System.Threading.Tasks;
using MartinBartos.AzureCognitiveSearch.Models;

namespace MartinBartos.AzureCognitiveSearch.Services
{
    public interface IIndexStrategy
    {
        Task CreateIndexAsync(string indexName, IList<ProductModel> data);
    }
}