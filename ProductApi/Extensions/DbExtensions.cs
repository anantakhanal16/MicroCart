using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductApi.ApplicationDbContext;
using System;

namespace ProductApi.Extensions
{
    public static class DbExtensions
    {
        public static IServiceCollection AddAppDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
            var dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "3306";
            var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "ProductDb";
            var dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? "root";
            var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "yourStrong(!)Password";

            var connectionString = $"server={dbHost};port={dbPort};database={dbName};user={dbUser};password={dbPassword}";

            services.AddDbContext<ProductDbContext>(options =>
                options.UseMySQL(connectionString));

            return services;
        }
    }
}
