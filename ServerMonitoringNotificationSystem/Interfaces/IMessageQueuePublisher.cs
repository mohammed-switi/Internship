namespace ServerMonitoringNotificationSystem.Interfaces;

public interface IMessageQueuePublisher
{
    Task PublishAsync(string routingKey, object message);
    Task ConnectAsync();
    Task DisconnectAsync();
}