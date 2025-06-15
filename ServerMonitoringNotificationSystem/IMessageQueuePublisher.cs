namespace ServerMonitoringNotificationSystem;

public interface IMessageQueuePublisher
{
    Task PublishAsync(string topic, object message);
    Task ConnectAsync();
    Task DisconnectAsync();
}