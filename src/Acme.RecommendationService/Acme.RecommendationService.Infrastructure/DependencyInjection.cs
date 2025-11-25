using Acme.RecommendationService.Application.Interfaces;
using Acme.RecommendationService.Application.Services;
using Acme.RecommendationService.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Acme.RecommendationService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<IRecommendationRepository, RecommendationRepository>();
            services.AddSingleton<IRecommendationServices, RecommendationServices>();
            return services;
        }
    }
}
