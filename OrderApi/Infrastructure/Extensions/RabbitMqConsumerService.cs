using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class RabbitMqConsumerService : BackgroundService
{
    private readonly ILogger<RabbitMqConsumerService> logger;
    private readonly ConnectionFactory factory;
    private readonly string exchangeName;
    private readonly string queueName;
    private readonly string routingKey;

    public RabbitMqConsumerService(
        ILogger<RabbitMqConsumerService> logger,
        IConfiguration configuration)
    {
        this.logger = logger;

        exchangeName = "order.exchange";
        queueName = "order.created.queue";
        routingKey = "order.created";

        factory = new ConnectionFactory
        {
            HostName = "rabbitmq",
            UserName = "guest",
            Password = "guest",
            AutomaticRecoveryEnabled = true
        };
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await using var connection = await factory.CreateConnectionAsync(cancellationToken);
        await using var channel = await connection.CreateChannelAsync();

        // 1️⃣ Declare Exchange (You were missing this)
        await channel.ExchangeDeclareAsync(
            exchange: exchangeName,
            type: ExchangeType.Topic,
            durable: true,
            autoDelete: false
        );

        // 2️⃣ Declare Queue
        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false
        );

        // 3️⃣ Bind Queue to Exchange
        await channel.QueueBindAsync(
            queue: queueName,
            exchange: exchangeName,
            routingKey: routingKey
        );

        var consumer = new AsyncEventingBasicConsumer(channel);
        logger.LogInformation("RabbitMQ consumer started. Waiting for messages...");

        consumer.ReceivedAsync += async (_, eventArgs) =>
        {
            var body = eventArgs.Body.ToArray();
            var messageJson = Encoding.UTF8.GetString(body);

            logger.LogInformation("Received message: {Message}", messageJson);

            await channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
        };

        await channel.BasicConsumeAsync(queueName, autoAck: false, consumer);
        await Task.Delay(Timeout.Infinite, cancellationToken);
    }
}
