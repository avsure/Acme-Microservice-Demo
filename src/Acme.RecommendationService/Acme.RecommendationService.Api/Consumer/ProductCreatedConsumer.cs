using Acme.Contracts;
using MassTransit;

namespace Acme.RecommendationService.Api.Consumer
{
    public class ProductCreatedConsumer : IConsumer<IProductCreated>
    {
        // inject services (db, recommendation engine) via ctor
        public async Task Consume(ConsumeContext<IProductCreated> context)
        {
            var msg = context.Message;
            // store product info, update recommendation indexes, etc.
            Console.WriteLine($"Received ProductCreated: {msg.ProductId} - {msg.Name}");
            await Task.CompletedTask;
        }
    }

}
