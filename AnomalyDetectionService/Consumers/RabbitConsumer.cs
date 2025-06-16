using System.Text;
using System.Text.Json;
using AnomalyDetectionService.Interfaces;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AnomalyDetectionService.Consumers;

public class RabbitMqConsumer<T>(
    string hostname,
    string exchangeName,
    ILogger<RabbitMqConsumer<T>> logger,
    string routingKeyPattern
    )
    : IMessageConsumer<T>
{
    
    
    private IConnection _connection = null!;
    private IChannel _channel = null!;
    private CancellationTokenSource? _cts;
public event Func<object, T, Task>? OnMessageReceived;

private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };
    

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
        if (_cts != null) await _cts.CancelAsync();
        await _channel.CloseAsync();
        await _connection.CloseAsync();
    }


    private async Task ConsumeAsync(CancellationToken cancellationToken)
    {
        try
        {
            await SetupMessagingInfrastructureAsync(cancellationToken);
            await StartConsumingMessagesAsync(cancellationToken);

            while (!cancellationToken.IsCancellationRequested)
                await Task.Delay(1000, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            logger.LogError( "[RabbitMqConsumer] Consumption was cancelled.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "[RabbitMqConsumer] An error occurred while consuming messages.");
            throw;
        }
    }

    private async Task SetupMessagingInfrastructureAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("[RabbitMqConsumer] Setting up messaging infrastructure...");
        
        await _channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Topic, true,
            cancellationToken: cancellationToken);
        await _channel.QueueDeclareAsync("server_stats_queue", true, false, false,
            cancellationToken: cancellationToken);
        await _channel.QueueBindAsync("server_stats_queue", exchangeName, routingKeyPattern,
            cancellationToken: cancellationToken);
        
        logger.LogInformation("[RabbitMqConsumer] Messaging infrastructure setup completed.");
    }

    private async Task StartConsumingMessagesAsync(CancellationToken cancellationToken)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);
        
        consumer.ReceivedAsync += async (model, ea) => 
            await HandleMessageAsync(ea, cancellationToken);
        
        
        await _channel.BasicConsumeAsync("server_stats_queue", false, consumer, cancellationToken);
        
        logger.LogInformation("[RabbitMqConsumer] Message consumption started.");
    }

    private async Task HandleMessageAsync(BasicDeliverEventArgs ea, CancellationToken cancellationToken)
    {
        logger.LogInformation("[RabbitMqConsumer] Received message with delivery tag: {DeliveryTag}", ea.DeliveryTag);
        var message = Encoding.UTF8.GetString(ea.Body.ToArray());
        Console.WriteLine($"[RabbitMqConsumer] Raw JSON: {message}");

        try
        {
            var data = JsonSerializer.Deserialize<T>(message, JsonOptions);

            if (data == null)
            {
                logger.LogInformation("[RabbitMqConsumer] Deserialized data is null");
                await HandleInvalidMessageAsync(ea, cancellationToken);
                return;
            }
            
            logger.LogInformation("[RabbitMqConsumer] Deserialized data successfully : {Data}", JsonSerializer.Serialize(data, JsonOptions));

            if (OnMessageReceived != null)
                await OnMessageReceived.Invoke(this, data);

            await _channel.BasicAckAsync(ea.DeliveryTag, false, cancellationToken);
            Console.WriteLine("[RabbitMqConsumer] Message acknowledged successfully");
        }
        catch (JsonException ex)
        {
            logger.LogError(ex, "[RabbitMqConsumer] JSON deserialization failed for message: {Message}", message);
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
        logger.LogWarning("[RabbitMqConsumer] Invalid message received. Nacking message with delivery tag: {DeliveryTag}", ea.DeliveryTag);
        await _channel.BasicNackAsync(ea.DeliveryTag, false, false, cancellationToken);
    }
}