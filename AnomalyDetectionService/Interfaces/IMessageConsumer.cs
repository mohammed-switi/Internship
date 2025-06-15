namespace AnomalyDetectionService;

public interface IMessageConsumer<T>
{
    Task StartConsumingAsync();

    Task StopConsumingAsync();

    event Func<object, T, Task>? onMessageReceived;
}