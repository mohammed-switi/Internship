using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AnomalyDetectionService;

public class RabbitMqConsumer<T>(
    IConnectionFactory connectionFactory,
    string exchangeName,
    string queueName,
    string routingKeyPattern = "ServerStatistics.*")
    : IMessageConsumer<T>
{
    private readonly IConnectionFactory _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
    private readonly string _exchangeName = exchangeName ?? throw new ArgumentNullException(nameof(exchangeName));
    private readonly string _queueName = queueName ?? throw new ArgumentNullException(nameof(queueName));
    private readonly string _routingKeyPattern = routingKeyPattern;

    private IConnection _connection= null!;
    private IChannel _channel= null!;
    private CancellationTokenSource? _cts;

    // ✅ Matching the interface event name (lowercase)
    public event EventHandler<T>? onMessageReceived;

    public void startConsuming()
    {
        _cts = new CancellationTokenSource();
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
            _connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

            await _channel.ExchangeDeclareAsync(_exchangeName, ExchangeType.Topic, durable: true, cancellationToken: cancellationToken);
            await _channel.QueueDeclareAsync(_queueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: cancellationToken);
            await _channel.QueueBindAsync(_queueName, _exchangeName, _routingKeyPattern, cancellationToken: cancellationToken);

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

            await _channel.BasicConsumeAsync(_queueName, autoAck: false, consumer: consumer, cancellationToken: cancellationToken);

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