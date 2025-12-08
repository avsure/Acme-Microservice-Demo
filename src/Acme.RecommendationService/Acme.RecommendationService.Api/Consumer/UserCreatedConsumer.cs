using Acme.Contracts;
using MassTransit;

namespace Acme.RecommendationService.Api.Consumer
{
    public class UserCreatedConsumer : IConsumer<IUserCreated>
    {
        public async Task Consume(ConsumeContext<IUserCreated> context)
        {
            var msg = context.Message;
            // create initial user profile for recommendations, seed data etc.
            Console.WriteLine($"Received UserCreated: {msg.UserId} - {msg.Email}");
            await Task.CompletedTask;
        }
    }

}
