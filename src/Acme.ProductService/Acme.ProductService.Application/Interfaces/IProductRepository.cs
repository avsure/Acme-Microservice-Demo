using Acme.ProductService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.ProductService.Application.Interfaces
{
    public interface IProductRepository
    {
        Task AddAsync(Product product);
        Task<Product?> GetByIdAsync(Guid id);
        Task<IEnumerable<Product>> GetAllAsync();
    }
}
