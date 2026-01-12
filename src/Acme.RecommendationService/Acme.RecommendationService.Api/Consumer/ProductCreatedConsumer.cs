using Acme.Contracts;
using MassTransit;
using Serilog.Context;
using Microsoft.Extensions.Logging;

namespace Acme.RecommendationService.Api.Consumer
{
    public class ProductCreatedConsumer : IConsumer<IProductCreated>
    {
        private readonly ILogger<ProductCreatedConsumer> _logger;

        public ProductCreatedConsumer(ILogger<ProductCreatedConsumer> logger)
        {
            _logger = logger;
        }

        // inject services (db, recommendation engine) via ctor
        public async Task Consume(ConsumeContext<IProductCreated> context)
        {

            try
            {
                var correlationId = context.Headers.Get<string>("X-Correlation-ID")
                                ?? Guid.NewGuid().ToString();

                using (Serilog.Context.LogContext.PushProperty("CorrelationId", correlationId))
                {
                    _logger.LogInformation("Consumed ProductCreated event");
                }

                var msg = context.Message;

                Console.WriteLine($"Received ProductCreated: {msg.ProductId} - {msg.Name}");

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while consuming ProductCreated event");
                throw; // MassTransit retry policy will handle it
            }
        }
    }
}
