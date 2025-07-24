using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OrderApi.MongoDbConfigurations;

namespace ProductApi.Extensions
{
    public static class DbExtensions
    {
        public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            var dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
            var dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "27017";
            var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "ProductDb";
            var dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? "";
            var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "";

            var credentials = string.IsNullOrWhiteSpace(dbUser) ? "" : $"{dbUser}:{dbPassword}@";
            var connectionString = $"mongodb://{credentials}{dbHost}:{dbPort}";

            // Register MongoDB settings
            services.Configure<MongoDbSettings>(options =>
            {
                options.ConnectionString = connectionString;
                options.DatabaseName = dbName;
            });

            // Register MongoDB client
            services.AddSingleton<IMongoClient>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
                return new MongoClient(settings.ConnectionString);
            });

            return services;
        }
    }
}
