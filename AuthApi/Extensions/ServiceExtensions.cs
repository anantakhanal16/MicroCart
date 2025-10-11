using AuthApi.Interface;
using AuthApi.Services;
using JwtAuthenticationManager;

namespace AuthApi.Extensions
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
