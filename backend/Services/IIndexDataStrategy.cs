using System.Collections.Generic;
using System.Threading.Tasks;
using MartinBartos.AzureCognitiveSearch.Models;

namespace MartinBartos.AzureCognitiveSearch.Services
{
    public interface IIndexStrategy
    {
        Task CreateIndexAsync(IList<ProductModel> data);
        Task<IEnumerable<ProductModel>> SearchAsync(string query);
    }
}