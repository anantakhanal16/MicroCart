using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Messaging.Dto;
using Messaging.Interfaces;
using Messaging.Models;
using RabbitMQ.Client;

namespace Messaging.Services
{
    public class RabbitMqService : IRabbitMqService
    {
        private readonly ConnectionFactory _factory;

        public RabbitMqService(RabbitMqSettingsConfig settings)
        {
            _factory = new ConnectionFactory()
            {
                HostName = settings.HostName,
                UserName = settings.UserName,
                Password = settings.Password
            };
        }

        public async Task Publish<T>(RabbitMqMessage<T> message, CancellationToken cancellationToken)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (string.IsNullOrWhiteSpace(message.Exchange)) throw new ArgumentNullException(nameof(message.Exchange));
            if (string.IsNullOrWhiteSpace(message.RoutingKey)) throw new ArgumentNullException(nameof(message.RoutingKey));

            await using var connection = await _factory.CreateConnectionAsync(cancellationToken);
            await using var channel = await connection.CreateChannelAsync();

            // Declare exchange (topic type is common for microservices)
            await channel.ExchangeDeclareAsync(message.Exchange, ExchangeType.Topic, durable: true);

            // Serialize message to JSON
            var bodyBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
            var body = new ReadOnlyMemory<byte>(bodyBytes);

            // Publish message
            var properties = new BasicProperties();
            await channel.BasicPublishAsync(
                exchange: message.Exchange,
                routingKey: message.RoutingKey,
                mandatory: false,
                basicProperties: properties,
                body: body);

            Console.WriteLine($"Message published to exchange '{message.Exchange}' with routing key '{message.RoutingKey}'.");
        }
    }
}