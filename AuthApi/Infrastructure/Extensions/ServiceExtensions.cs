using AuthApi.Application.Interface;
using AuthApi.Infrastructure.Services;
using JwtAuthenticationManager;

namespace AuthApi.Infrastructure.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IJwtTokenHandler, JwtTokenHandler>();

    
            services.AddScoped<IAccountService, AccountServices>();

            return services;
        }
    }
}
