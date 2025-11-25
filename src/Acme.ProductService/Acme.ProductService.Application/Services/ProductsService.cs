using Acme.ProductService.Application.DTOs;
using Acme.ProductService.Application.Interfaces;
using Acme.ProductService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.ProductService.Application.Services
{
    public class ProductsService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductsService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _repository.GetAllAsync();

            return products.Select(p => new ProductDto(
                p.Id, p.Name, p.Category, p.Price, p.Description
            ));
        }

        public async Task<ProductDto?> GetProductByIdAsync(Guid id)
        {
            var product = await _repository.GetByIdAsync(id);

            return product is null
                ? null
                : new ProductDto(product.Id, product.Name, product.Category, product.Price, product.Description);
        }

        public async Task<Guid> CreateProductAsync(ProductCreateModel request)
        {
            var product = new Product(
                Guid.NewGuid(),
                request.Name,
                request.Category,
                request.Price,
                request.Description
            );

            await _repository.AddAsync(product);
            return product.Id;
        }
    }
}
