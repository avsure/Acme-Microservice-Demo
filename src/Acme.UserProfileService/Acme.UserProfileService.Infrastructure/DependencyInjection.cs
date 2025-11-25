using Acme.UserProfileService.Application.Interfaces;
using Acme.UserProfileService.Application.Services;
using Acme.UserProfileService.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.UserProfileService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IUserProfileRepository, InMemoryUserProfileRepository>();
            services.AddScoped<IUserProfileService, UserProfileServices>();
            return services;
        }
    }
}
