using CustomerWebApi.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CustomerWebApi.Extensions
{
    public static class DbExtensions
    {
        public static IServiceCollection AddAppDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
            var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "CustomerDb";
            var dbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD") ?? "yourStrong(!)Password";

            var connectionString = $"Server={dbHost};Database={dbName};User Id=sa;Password={dbPassword};TrustServerCertificate=true;MultipleActiveResultSets=true;";

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            return services;
        }
    }
}
