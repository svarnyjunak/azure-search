using System;

namespace MartinBartos.AzureCognitiveSearch.Models
{
    public class ProductModel : IProductModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
    }
}