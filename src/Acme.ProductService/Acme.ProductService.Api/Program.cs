using Acme.ProductService.Application.Interfaces;
using Acme.ProductService.Application.Services;
using Acme.ProductService.Infrastructure;
using Acme.ProductService.Infrastructure.Persistence;
using Acme.ProductService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

public partial class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //Controllers
        builder.Services.AddControllers();

        //DbContext
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseInMemoryDatabase("ProductDb"));

        //Dependency Injection
        builder.Services.AddInfrastructure();
        //builder.Services.AddScoped<IProductRepository, ProductRepository>();
        //builder.Services.AddScoped<IProductService, ProductsService>();

        //Swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();

        app.MapControllers(); // Enables conventional routing

        app.Run();
    }
}