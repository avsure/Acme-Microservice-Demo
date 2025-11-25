using Acme.ProductService.Application.Interfaces;
using Acme.ProductService.Application.Services;
using Acme.ProductService.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Acme.ProductService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository,ProductRepository>();
            services.AddScoped<IProductService, ProductsService>();

            return services;
        }
    }
}
