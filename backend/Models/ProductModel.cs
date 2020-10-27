using System;

namespace MartinBartos.AzureCognitiveSearch.Models
{
    public class ProductModel
    {
        public string Id { get; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
    }
}