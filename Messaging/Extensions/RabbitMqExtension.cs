
using System;
using Messaging.Dto;
using Messaging.Interfaces;
using Messaging.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Messaging.Extensions
{
    public static class RabbitMqExtensions
    {
        public static IServiceCollection AddRabbitMq(this IServiceCollection services)
        {
            // Read RabbitMQ configuration from environment variables or fallback defaults
            var host = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";
            var user = Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? "guest";
            var password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? "guest";


            var settings = new RabbitMqSettingsConfig
            {
                HostName = host,
                UserName = user,
                Password = password
            };

         
            services.AddSingleton<IRabbitMqService>(sp => new RabbitMqService(settings));

            return services;
        }
    }
}

