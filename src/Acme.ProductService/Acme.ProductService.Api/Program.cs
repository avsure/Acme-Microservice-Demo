using Acme.ProductService.Api.Middleware;
using Acme.ProductService.Infrastructure;
using Acme.ProductService.Infrastructure.Persistence;
using MassTransit;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.EntityFrameworkCore;
using Serilog;

public partial class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAngular",
                policy =>
                {
                    policy.WithOrigins("http://localhost:4200")
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
        });

        // Detect environment
        var env = builder.Environment.EnvironmentName;

        // --- RabbitMQ / MassTransit ---
        if (!env.Equals("Test", StringComparison.OrdinalIgnoreCase))
        {
            var rabbitHost = builder.Configuration.GetValue<string>("RabbitMq:Host") ?? "localhost";
            var rabbitUser = builder.Configuration.GetValue<string>("RabbitMq:User") ?? "guest";
            var rabbitPass = builder.Configuration.GetValue<string>("RabbitMq:Pass") ?? "guest";

            builder.Services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitHost, "/", h =>
                    {
                        h.Username(rabbitUser);
                        h.Password(rabbitPass);
                    });

                    // Optional: endpoint name formatter
                    cfg.ConfigureEndpoints(context);
                });
            });

        }
        else
        {
            // Test environment → skip real RabbitMQ, optional in-memory harness
            builder.Services.AddMassTransitTestHarness();
        }

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

        // Application Insights
        builder.Services.AddApplicationInsightsTelemetry();

        builder.Logging.ClearProviders();

        // Serilog & CorrelationId
        builder.Host.UseSerilog((context, services, loggerConfiguration) =>
        {
            loggerConfiguration
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Acme.ProductService", "ProductService") // change per service
                .WriteTo.Console()
                .WriteTo.ApplicationInsights(
                    services.GetRequiredService<TelemetryConfiguration>(),
                    TelemetryConverter.Traces);
        });

        builder.Logging.AddSerilog();

        builder.Host.UseSerilog();

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddTransient<CorrelationIdHandler>();

        builder.Services.AddHttpClient("Default")
            .AddHttpMessageHandler<CorrelationIdHandler>();

        var app = builder.Build();

        app.UseMiddleware<CorrelationIdMiddleware>();

        app.UseCors("AllowAngular");

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