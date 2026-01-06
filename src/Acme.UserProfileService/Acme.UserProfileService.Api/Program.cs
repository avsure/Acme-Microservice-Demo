using Acme.UserProfileService.Api.Middleware;
using Acme.UserProfileService.Infrastructure;
using MassTransit;
using Microsoft.ApplicationInsights.Extensibility;
using Serilog;

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

//application Insights
builder.Services.AddApplicationInsightsTelemetry();

// Detect environment
var env = builder.Environment.EnvironmentName;

if (!env.Equals("Test", StringComparison.OrdinalIgnoreCase))
{
    var rabbitHost = builder.Configuration.GetValue<string>("RabbitMq:Host") ?? "localhost";
    var rabbitUser = builder.Configuration.GetValue<string>("RabbitMq:User") ?? "guest";
    var rabbitPass = builder.Configuration.GetValue<string>("RabbitMq:Pass") ?? "guest";

    builder.Services.AddMassTransit(x =>
    {
        // No consumers here, only configure RabbitMQ so we can publish.
        x.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host(rabbitHost, "/", h =>
            {
                h.Username(rabbitUser);
                h.Password(rabbitPass);
            });

            // optional: endpoint name formatter
            cfg.ConfigureEndpoints(context);
        });
    });
}
else
{
    // Test environment → skip real RabbitMQ, optional in-memory harness
    builder.Services.AddMassTransitTestHarness();
}

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddInfrastructure();

// Application Insights
builder.Services.AddApplicationInsightsTelemetry();

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

builder.Host.UseSerilog();

builder.Services.AddHttpContextAccessor();

builder.Services.AddTransient<CorrelationIdHandler>();

builder.Services.AddHttpClient("Default")
           .AddHttpMessageHandler<CorrelationIdHandler>();

var app = builder.Build();

app.UseMiddleware<CorrelationIdMiddleware>();

app.UseCors("AllowAngular");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
