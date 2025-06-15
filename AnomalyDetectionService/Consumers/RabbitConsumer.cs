using System.Text;
using System.Text.Json;
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

    private IConnection _connection= null!;
    private IChannel _channel= null!;
    private CancellationTokenSource? _cts;

    public event EventHandler<T>? onMessageReceived;

    public async void startConsuming()
    {
        _cts = new CancellationTokenSource();
        var factory = new ConnectionFactory() { HostName = hostname };
        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();
        
        Task.Run(() => ConsumeAsync(_cts.Token));
    }

    public  void stopConsuming()
    {
        _cts?.Cancel();
        _channel.CloseAsync();
        _connection.CloseAsync();
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
                    var data = JsonSerializer.Deserialize<T>(message);
                    if (data != null)
                    {
                        onMessageReceived?.Invoke(this, data);
                        
                        await _channel.BasicAckAsync(ea.DeliveryTag, false, cancellationToken);
                    }
                    else
                    {
                        HandleInvalidMessage(ea);
                    }
                }
                catch (JsonException)
                {
                    Console.Error.WriteLine("[RabbitMqConsumer] JSON deserialization failed.");
                    HandleInvalidMessage(ea);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"[RabbitMqConsumer] Error: {ex.Message}");
                    // Retry / log if necessary
                }

                await Task.Yield();
            };

            await _channel.BasicConsumeAsync("server_stats_queue", autoAck: false, consumer: consumer, cancellationToken: cancellationToken);

            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(500);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[RabbitMqConsumer] Fatal error: {ex.Message}");
        }
    }

    private void HandleInvalidMessage(BasicDeliverEventArgs ea)
    {
        _channel.BasicNackAsync(ea.DeliveryTag, false, requeue: false);
    }
}