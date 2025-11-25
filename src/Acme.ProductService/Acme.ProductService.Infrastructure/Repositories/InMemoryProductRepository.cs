using Acme.ProductService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.ProductService.Infrastructure.Repositories
{
    public static class InMemoryProductRepository 
    {
        public static readonly List<Product> ProductList = new List<Product>
        {
            new Product(
                Guid.Parse("11111111-1111-1111-1111-111111111111"),
                "Wireless Mouse",
                "Electronics",
                1500.00m,
                "A wireless mouse with ergonomic design"
            ),
            new Product(
                Guid.Parse("22222222-2222-2222-2222-222222222222"),
                "LED Monitor",
                "Electronics",
                8500.50m,
                "24-inch LED monitor full HD"
            ),
            new Product(
                Guid.Parse("33333333-3333-3333-3333-333333333333"),
                "Running Shoes",
                "Footwear",
                2999.99m,
                "Lightweight and durable shoes"
            )
        };
    }
}
