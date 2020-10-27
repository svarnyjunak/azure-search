using System;

namespace MartinBartos.AzureCognitiveSearch.Models
{
    public class ProductModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
    }
}