using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AnomalyDetectionService;

public class RabbitMqConsumer<T>(
    string hostname,
    string exchangeName,
    string routingKeyPattern = "ServerStatistics.*")
    : IMessageConsumer<T>
{
    private IConnection _connection = null!;
    private IChannel _channel = null!;
    private CancellationTokenSource? _cts;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };

    public event Func<object, T, Task>? onMessageReceived;

    public async Task StartConsumingAsync()
    {
        _cts = new CancellationTokenSource();
        var factory = new ConnectionFactory() { HostName = hostname };
        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        await ConsumeAsync(_cts.Token);
    }

    public async Task StopConsumingAsync()
    {
        _cts?.Cancel();
        if (_channel != null)
            await _channel.CloseAsync();
        if (_connection != null)
            await _connection.CloseAsync();
    }


    private async Task ConsumeAsync(CancellationToken cancellationToken)
    {
        try
        {
            await SetupMessagingInfrastructureAsync(cancellationToken);
            await StartConsumingMessagesAsync(cancellationToken);

            // Keep the consumer alive
            while (!cancellationToken.IsCancellationRequested)
                await Task.Delay(1000, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("[RabbitMqConsumer] Consumption cancelled");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[RabbitMqConsumer] Fatal error: {ex.Message}");
            throw;
        }
    }

    private async Task SetupMessagingInfrastructureAsync(CancellationToken cancellationToken)
    {
        await _channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Topic, true,
            cancellationToken: cancellationToken);
        await _channel.QueueDeclareAsync("server_stats_queue", true, false, false,
            cancellationToken: cancellationToken);
        await _channel.QueueBindAsync("server_stats_queue", exchangeName, routingKeyPattern,
            cancellationToken: cancellationToken);
    }

    private async Task StartConsumingMessagesAsync(CancellationToken cancellationToken)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (model, ea) => await HandleMessageAsync(ea, cancellationToken);
        await _channel.BasicConsumeAsync("server_stats_queue", false, consumer, cancellationToken);
    }

    private async Task HandleMessageAsync(BasicDeliverEventArgs ea, CancellationToken cancellationToken)
    {
        var message = Encoding.UTF8.GetString(ea.Body.ToArray());
        Console.WriteLine($"[RabbitMqConsumer] Raw JSON: {message}");

        try
        {
            var data = JsonSerializer.Deserialize<T>(message, JsonOptions);

            if (data == null)
            {
                Console.Error.WriteLine("[RabbitMqConsumer] Deserialized data is null");
                await HandleInvalidMessageAsync(ea, cancellationToken);
                return;
            }

            Console.WriteLine(
                $"[RabbitMqConsumer] Deserialized successfully: {JsonSerializer.Serialize(data, JsonOptions)}");

            if (onMessageReceived != null)
                await onMessageReceived.Invoke(this, data);

            await _channel.BasicAckAsync(ea.DeliveryTag, false, cancellationToken);
            Console.WriteLine("[RabbitMqConsumer] Message acknowledged successfully");
        }
        catch (JsonException ex)
        {
            Console.Error.WriteLine($"[RabbitMqConsumer] JSON deserialization failed: {ex.Message}");
            await HandleInvalidMessageAsync(ea, cancellationToken);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[RabbitMqConsumer] Error processing message: {ex.Message}");
            await _channel.BasicNackAsync(ea.DeliveryTag, false, false, cancellationToken);
        }
    }

    private async Task HandleInvalidMessageAsync(BasicDeliverEventArgs ea, CancellationToken cancellationToken)
    {
        await _channel.BasicNackAsync(ea.DeliveryTag, false, false, cancellationToken);
    }
}