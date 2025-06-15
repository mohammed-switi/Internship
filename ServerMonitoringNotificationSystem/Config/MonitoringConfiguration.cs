namespace ServerMonitoringNotificationSystem;

public class MonitoringConfiguration
{
    public int SamplingIntervalSeconds { get; set; } = 30;
    public string ServerIdentifier { get; set; } = Environment.MachineName;
    public string MessageQueueConnectionString { get; set; } = "localhost";
    public string TopicPrefix { get; set; } = "ServerStatistics";
    public string MessageQueueType { get; set; } = "Console"; // Console, RabbitMQ, ServiceBus
    public bool EnableLogging { get; set; } = true;
    public int MaxRetryAttempts { get; set; } = 3;

    public void Validate()
    {
        if (SamplingIntervalSeconds <= 0)
            throw new ArgumentException("SamplingIntervalSeconds must be greater than 0");
        
        if (string.IsNullOrWhiteSpace(ServerIdentifier))
            throw new ArgumentException("ServerIdentifier cannot be null or empty");
        
        if (string.IsNullOrWhiteSpace(TopicPrefix))
            throw new ArgumentException("TopicPrefix cannot be null or empty");
    }

    public override string ToString()
    {
        return $"Server: {ServerIdentifier}, Interval: {SamplingIntervalSeconds}s, " +
               $"Queue: {MessageQueueType}, Topic: {TopicPrefix}";
    }
}