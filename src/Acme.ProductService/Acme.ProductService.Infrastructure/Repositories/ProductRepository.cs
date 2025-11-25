using Acme.ProductService.Application.Interfaces;
using Acme.ProductService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.ProductService.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly List<Product> _products = [];

        public Task AddAsync(Product product)
        {
            InMemoryProductRepository.ProductList.Add(product);
            return Task.CompletedTask;
        }

        public Task<Product?> GetByIdAsync(Guid id)
        {
            var product = InMemoryProductRepository.ProductList.FirstOrDefault(p => p.Id == id);
            return Task.FromResult(product);
        }

        public Task<IEnumerable<Product>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Product>>(InMemoryProductRepository.ProductList);
        }
    }
}
