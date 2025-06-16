using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ServerMonitoringNotificationSystem.Config;
using ServerMonitoringNotificationSystem.Interfaces;

namespace ServerMonitoringNotificationSystem.Services;

public class ServerStatisticsCollectionService(
    ISystemStatisticsCollector statisticsCollector,
    IMessageQueuePublisher messagePublisher,
    MonitoringConfiguration config,
    ILogger<ServerStatisticsCollectionService> logger)
    : BackgroundService
{
    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting Server Statistics Collection Service");
        logger.LogInformation("Server Identifier: {ServerIdentifier}", config.ServerIdentifier);
        logger.LogInformation("Sampling Interval: {Interval} seconds", config.SamplingIntervalSeconds);
        
        await messagePublisher.ConnectAsync();
        await base.StartAsync(cancellationToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Stopping Server Statistics Collection Service");
        await messagePublisher.DisconnectAsync();
        await base.StopAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // collect statistics
                var statistics = await statisticsCollector.CollectStatisticsAsync(config.ServerIdentifier);
                
                // create topic name
                var topic = $"{config.TopicPrefix}.{config.ServerIdentifier}";
                
                // publish to message queue
                await messagePublisher.PublishAsync(topic, statistics);
                
                logger.LogInformation("Published statistics for server: {ServerIdentifier}", config.ServerIdentifier);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in statistics collection cycle");
            }

            // Wait for the next sampling interval
            await Task.Delay(TimeSpan.FromSeconds(config.SamplingIntervalSeconds), stoppingToken);
        }
    }
}
