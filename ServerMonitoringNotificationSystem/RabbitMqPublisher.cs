using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace ServerMonitoringNotificationSystem;

public class RabbitMqPublisher(string connectionString, ILogger<RabbitMqPublisher> logger) : IMessageQueuePublisher
{
    private IConnection _connection = null!;
    private IChannel _channel = null!;

    public async Task ConnectAsync()
    {
        var factory = new ConnectionFactory() { HostName = connectionString };
        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();
        logger.LogInformation("Connected to RabbitMQ");
    }

    public async Task PublishAsync(string topic, object message)
    {
        await _channel.ExchangeDeclareAsync("server_monitoring", ExchangeType.Topic);

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        await _channel.BasicPublishAsync(
            "server_monitoring",
            topic,
            false,
            body
        );

        logger.LogInformation("Published message to topic: {Topic}", topic);
    }

    public async Task DisconnectAsync()
    {
        await _channel.CloseAsync();
        await _connection.CloseAsync();
        logger.LogInformation("Disconnected from RabbitMQ");
    }
}