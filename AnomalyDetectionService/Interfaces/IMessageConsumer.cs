namespace AnomalyDetectionService.Interfaces;

public interface IMessageConsumer<T>
{
    Task StartConsumingAsync();

    Task StopConsumingAsync();

    event Func<object, T, Task>? OnMessageReceived;
}