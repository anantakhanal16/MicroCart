using AuthApi.Application.Interface;
using AuthApi.Infrastructure.Services;
using JwtAuthenticationManager;
using JwtAuthenticationManager.ServiceExtensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthApi.Infrastructure.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            var customerServiceUrl = configuration["CustomerService:BaseUrl"];

            services.AddScoped<IJwtTokenHandler, JwtTokenHandler>();
            services.AddHttpClient<IAccountService, AccountServices>(client =>
            {
                client.BaseAddress = new Uri(customerServiceUrl!);
            });
            services.AddCustomJwtAuthExtension();
            return services;
        }
    }
}