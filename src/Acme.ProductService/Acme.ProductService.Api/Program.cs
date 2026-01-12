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

        builder.Logging.ClearProviders();

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

        #region RabbitMQ / MassTransit

        // --- RabbitMQ / MassTransit --- keep this for local work on case
        var messagingProvider = builder.Configuration["Messaging:Provider"];
        Log.Information("Inside Product Service: Messaging Provider: {Provider}", messagingProvider);

        if (messagingProvider == "RabbitMQ")
        {
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
        }
        else
        {
            // ✅ Safe no-broker mode
            builder.Services.AddMassTransit(x =>
            {
                x.UsingInMemory((context, cfg) => { });
            });
        }

        #endregion

        #region Serilog & CorrelationId

        builder.Host.UseSerilog((context, services, config) =>
        {
            var aiConnectionString = context.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"];

            config
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Service", "ProductService")
                .WriteTo.Console()
                .WriteTo.ApplicationInsights(aiConnectionString, TelemetryConverter.Traces)
                .WriteTo.File("/home/LogFiles/log-.txt", rollingInterval: RollingInterval.Day);
        });

        #endregion

        //Controllers
        builder.Services.AddControllers();

        //DbContext
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseInMemoryDatabase("ProductDb"));

        //Dependency Injection
        builder.Services.AddInfrastructure();

        //Swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Application Insights
        builder.Services.AddApplicationInsightsTelemetry();

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddTransient<CorrelationIdHandler>();

        builder.Services.AddHttpClient("Default")
            .AddHttpMessageHandler<CorrelationIdHandler>();

        var app = builder.Build();

        Log.Information("ProductService API started successfully");

        app.UseMiddleware<CorrelationIdMiddleware>();

        app.UseCors("AllowAngular");

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();

        // Enables conventional routing
        app.MapControllers(); 

        app.Run();
    }
}