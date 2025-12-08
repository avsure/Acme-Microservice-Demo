using Acme.RecommendationService.Api.Consumer;
using Acme.RecommendationService.Infrastructure;
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

var rabbitHost = builder.Configuration.GetValue<string>("RabbitMq:Host") ?? "localhost";
var rabbitUser = builder.Configuration.GetValue<string>("RabbitMq:User") ?? "guest";
var rabbitPass = builder.Configuration.GetValue<string>("RabbitMq:Pass") ?? "guest";


builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ProductCreatedConsumer>();
    x.AddConsumer<UserCreatedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(rabbitHost, "/", h =>
        {
            h.Username(rabbitUser);
            h.Password(rabbitPass);
        });

        cfg.ReceiveEndpoint("recommendation-product-created", e =>
        {
            e.ConfigureConsumer<ProductCreatedConsumer>(context);
        });

        cfg.ReceiveEndpoint("recommendation-user-created", e =>
        {
            e.ConfigureConsumer<UserCreatedConsumer>(context);
        });

        // optional: cfg.ConfigureEndpoints(context);
    });
});



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
