using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.ProductService.Domain
{
    public class Product
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Category { get; private set; } = string.Empty;
        public decimal Price { get; private set; }
        public string Description { get; private set; } = string.Empty;

        // Private constructor for EF and serializers
        private Product() { }

        // Public factory-like constructor
        public Product(Guid id, string name, string category, decimal price, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Product name cannot be empty.", nameof(name));

            Id = id;
            Name = name;
            Category = category;
            Price = price;
            Description = description;
        }

        // Behavior — Domain logic
        public void UpdatePrice(decimal newPrice)
        {
            if (newPrice <= 0)
                throw new ArgumentException("Price must be > 0");

            Price = newPrice;
        }
    }

}
