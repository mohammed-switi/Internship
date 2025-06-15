namespace AnomalyDetectionService.Config;

public class RabbitMqConfig
{
    public string HostName { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string UserName { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string QueueName { get; set; } = "server_stats_queue";
    public string ExchangeName { get; set; } = "ServerExchange";
    public string RoutingKey { get; set; } = "ServerStatistics.*";
}