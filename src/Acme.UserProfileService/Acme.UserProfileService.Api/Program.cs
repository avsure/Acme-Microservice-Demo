using Acme.UserProfileService.Api.Middleware;
using Acme.UserProfileService.Infrastructure;
using MassTransit;
using Serilog;

internal class Program
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

        Log.Information("Inside Userprofile: Messaging Provider: {Provider}", messagingProvider);

        if (messagingProvider == "RabbitMQ")
        {
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
                .Enrich.WithProperty("Service", "UserProfileService")
                .WriteTo.Console()
                .WriteTo.ApplicationInsights(aiConnectionString, TelemetryConverter.Traces)
                .WriteTo.File("/home/LogFiles/log-.txt", rollingInterval: RollingInterval.Day);
        });

        #endregion

        // Add services to the container.
        builder.Services.AddControllers();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        //builder.Services.AddOpenApi();

        builder.Services.AddInfrastructure();

        //Swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Application Insights
        builder.Services.AddApplicationInsightsTelemetry();

        builder.Logging.ClearProviders();

        builder.Services.AddTransient<CorrelationIdHandler>();

        builder.Services.AddHttpClient("Default")
                   .AddHttpMessageHandler<CorrelationIdHandler>();

        builder.Services.AddHttpContextAccessor();

        var app = builder.Build();

        Log.Information("User profile Service API started successfully");

        app.UseMiddleware<CorrelationIdMiddleware>();

        app.UseCors("AllowAngular");

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            //app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}