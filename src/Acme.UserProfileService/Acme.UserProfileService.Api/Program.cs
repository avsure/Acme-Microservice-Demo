using Acme.UserProfileService.Infrastructure;
using MassTransit;

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

var app = builder.Build();

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
