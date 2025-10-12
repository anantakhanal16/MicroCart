using System.Text;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class RabbitMqConsumerService : BackgroundService
{
    private readonly ILogger<RabbitMqConsumerService> _logger;
    private readonly ConnectionFactory _factory;
    private readonly string _exchangeName;
    private readonly string _queueName;
    private readonly string _routingKey;

    public RabbitMqConsumerService(
        ILogger<RabbitMqConsumerService> logger,
        IConfiguration configuration)
    {
        _logger = logger;

        _exchangeName = configuration["RabbitMQ:ExchangeName"] ?? "default.exchange";
        _queueName = configuration["RabbitMQ:QueueName"] ?? "default.queue";
        _routingKey = configuration["RabbitMQ:RoutingKey"] ?? "default.routingKey";

        _factory = new ConnectionFactory
        {
            HostName = configuration["RabbitMQ:HostName"] ?? "rabbitmq",
            UserName = configuration["RabbitMQ:UserName"] ?? "guest",
            Password = configuration["RabbitMQ:Password"] ?? "guest",
            AutomaticRecoveryEnabled = true
        };
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await using var connection = await _factory.CreateConnectionAsync(cancellationToken);
        await using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: _queueName,
            durable: true,
            exclusive: false,
            autoDelete: false);

        var consumer = new AsyncEventingBasicConsumer(channel);
        _logger.LogInformation("RabbitMQ consumer started. Waiting for messages...");

        consumer.ReceivedAsync += async (_, eventArgs) =>
        {
            var body = eventArgs.Body.ToArray();
            var messageJson = Encoding.UTF8.GetString(body);

            _logger.LogInformation("Received message: {Message}", messageJson);

            await channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
        };

        await channel.BasicConsumeAsync(_queueName, autoAck: false, consumer);
        await Task.Delay(Timeout.Infinite, cancellationToken);
    }
}
