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
    private readonly string _exchangeName = exchangeName ?? throw new ArgumentNullException(nameof(exchangeName));
    private readonly string _routingKeyPattern = routingKeyPattern;
    private IConnection _connection = null!;
    private IChannel _channel = null!;
    private CancellationTokenSource? _cts;
    
    // JSON serializer options for camelCase property names
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
            await _channel.ExchangeDeclareAsync(_exchangeName, ExchangeType.Topic, durable: true, cancellationToken: cancellationToken);
            await _channel.QueueDeclareAsync("server_stats_queue", durable: true, exclusive: false, autoDelete: false, cancellationToken: cancellationToken);
            await _channel.QueueBindAsync("server_stats_queue", _exchangeName, _routingKeyPattern, cancellationToken: cancellationToken);
            
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                
                try
                {
                    Console.WriteLine($"[RabbitMqConsumer] Raw JSON: {message}");
                    
                    // Use the configured JsonOptions for proper deserialization
                    var data = JsonSerializer.Deserialize<T>(message, JsonOptions);
                    
                    if (data != null)
                    {
                        Console.WriteLine($"[RabbitMqConsumer] Deserialized successfully: {JsonSerializer.Serialize(data, JsonOptions)}");
                        
                        if (onMessageReceived != null)
                        {
                            await onMessageReceived.Invoke(this, data);
                        }
                        
                        await _channel.BasicAckAsync(ea.DeliveryTag, false, cancellationToken);
                        Console.WriteLine($"[RabbitMqConsumer] Message acknowledged successfully");
                    }
                    else
                    {
                        Console.Error.WriteLine("[RabbitMqConsumer] Deserialized data is null");
                        await HandleInvalidMessageAsync(ea, cancellationToken);
                    }
                }
                catch (JsonException ex)
                {
                    Console.Error.WriteLine($"[RabbitMqConsumer] JSON deserialization failed: {ex.Message}");
                    Console.Error.WriteLine($"[RabbitMqConsumer] Raw message: {message}");
                    await HandleInvalidMessageAsync(ea, cancellationToken);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"[RabbitMqConsumer] Error processing message: {ex.Message}");
                    await _channel.BasicNackAsync(ea.DeliveryTag, false, requeue: false, cancellationToken);
                }
            };
            
            await _channel.BasicConsumeAsync("server_stats_queue", autoAck: false, consumer: consumer, cancellationToken: cancellationToken);
            
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(1000, cancellationToken);
            }
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
    
    private async Task HandleInvalidMessageAsync(BasicDeliverEventArgs ea, CancellationToken cancellationToken)
    {
        await _channel.BasicNackAsync(ea.DeliveryTag, false, requeue: false, cancellationToken);
    }
}