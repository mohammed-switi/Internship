using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ServerMonitoringNotificationSystem;

public class ServerStatisticsCollectionService : BackgroundService
{
    private readonly ISystemStatisticsCollector _statisticsCollector;
    private readonly IMessageQueuePublisher _messagePublisher;
    private readonly MonitoringConfiguration _config;
    private readonly ILogger<ServerStatisticsCollectionService> _logger;

    public ServerStatisticsCollectionService(
        ISystemStatisticsCollector statisticsCollector,
        IMessageQueuePublisher messagePublisher,
        MonitoringConfiguration config,
        ILogger<ServerStatisticsCollectionService> logger)
    {
        _statisticsCollector = statisticsCollector;
        _messagePublisher = messagePublisher;
        _config = config;
        _logger = logger;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting Server Statistics Collection Service");
        _logger.LogInformation("Server Identifier: {ServerIdentifier}", _config.ServerIdentifier);
        _logger.LogInformation("Sampling Interval: {Interval} seconds", _config.SamplingIntervalSeconds);
        
        await _messagePublisher.ConnectAsync();
        await base.StartAsync(cancellationToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping Server Statistics Collection Service");
        await _messagePublisher.DisconnectAsync();
        await base.StopAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Collect statistics
                var statistics = await _statisticsCollector.CollectStatisticsAsync(_config.ServerIdentifier);
                
                // Create topic name
                var topic = $"{_config.TopicPrefix}.{_config.ServerIdentifier}";
                
                // Publish to message queue
                await _messagePublisher.PublishAsync(topic, statistics);
                
                _logger.LogInformation("Published statistics for server: {ServerIdentifier}", _config.ServerIdentifier);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in statistics collection cycle");
            }

            // Wait for the next sampling interval
            await Task.Delay(TimeSpan.FromSeconds(_config.SamplingIntervalSeconds), stoppingToken);
        }
    }
}
