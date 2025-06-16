using System.Text.Json;
using AnomalyDetectionService.Interfaces;
using AnomalyDetectionService.Models;

namespace AnomalyDetectionService;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class StatisticsProcessingService(
    IMongoRepository repository,
    IMessageConsumer<ServerStatistics> consumer,
    IAnomalyDetector detector,
    IAlertService alertService,
    ILogger<StatisticsProcessingService> logger)
    : BackgroundService
{
    public override Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("StatisticsProcessingService is starting.");
        consumer.onMessageReceived += async (sender, stats) =>
        {
            try
            {
                logger.LogInformation("Processing statistics message...");
                logger.LogInformation("Received stats: {Stats}", JsonSerializer.Serialize(stats));
                await HandleStatisticsAsync(stats);
                logger.LogInformation("Successfully processed statistics message");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing statistics message");
                throw;
            }
        };

        return consumer.StartConsumingAsync();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.CompletedTask;
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        consumer.StopConsumingAsync();
        logger.LogInformation("StatisticsProcessingService is stopping.");
        return base.StopAsync(cancellationToken);
    }


    private async Task HandleStatisticsAsync(ServerStatistics stats)
    {
        logger.LogInformation($"Received stats from {stats.ServerIdentifier} at {stats.Timestamp}");

        await repository.InsertAsync<ServerStatistics>(stats);

        var history = await GetOrderedRecentHistoryAsync(stats.ServerIdentifier);
        if (history.Count < 2)
        {
            logger.LogInformation("Not enough historical data for anomaly detection.");
            return;
        }

        var previous = history[1];

        await DetectAndSendAnomalyAlertAsync(stats, previous);
        await DetectAndSendHighUsageAlertAsync(stats);
    }

    private async Task<List<ServerStatistics>> GetOrderedRecentHistoryAsync(string serverIdentifier)
    {
        var history = await repository.GetRecentAsync<ServerStatistics>(serverIdentifier, 10);
        return history.OrderByDescending(s => s.Timestamp).ToList();
    }

   
private async Task DetectAndSendAnomalyAlertAsync(ServerStatistics current, ServerStatistics previous)
{
    var memAnomaly = detector.DetectMemoryAnomaly(current.MemoryUsage, previous.MemoryUsage);
    var cpuAnomaly = detector.DetectCpuAnomaly(current.CpuUsage, previous.CpuUsage);

    if (memAnomaly)
    {
        var memoryAlert = CreateAnomalyAlert(current, previous, "Memory");
        await alertService.SendAnomalyAlertAsync(memoryAlert);
        logger.LogWarning($"Anomaly detected: Memory on {current.ServerIdentifier}");
    }

    if (cpuAnomaly)
    {
        var cpuAlert = CreateAnomalyAlert(current, previous, "CPU");
        await alertService.SendAnomalyAlertAsync(cpuAlert);
        logger.LogWarning($"Anomaly detected: CPU on {current.ServerIdentifier}");
    }
}


private AnomalyAlert CreateAnomalyAlert(ServerStatistics current, ServerStatistics previous, string metricType)
{
    return new AnomalyAlert
    {
        ServerIdentifier = current.ServerIdentifier,
        MetricType = metricType,
        CurrentValue = metricType == "Memory" ? current.MemoryUsage : current.CpuUsage,
        PreviousValue = metricType == "Memory" ? previous.MemoryUsage : previous.CpuUsage,
        Timestamp = current.Timestamp
    };
}
    private async Task DetectAndSendHighUsageAlertAsync(ServerStatistics stats)
    {
        var highMemUsage = detector.IsHighUsageMemory(stats.MemoryUsage, stats.AvailableMemory);
        var highCpuUsage = detector.IsHighUsageCpu(stats.CpuUsage);

        if (!highMemUsage && !highCpuUsage) return;

        var highUsageAlert = new HighUsageAlert
        {
            ServerIdentifier = stats.ServerIdentifier,
            MetricType = highMemUsage ? "Memory" : "CPU",
            CurrentValue = highMemUsage ? stats.MemoryUsage : stats.CpuUsage,
            Threshold = highMemUsage
                ? detector.MemoryUsageThresholdPercentage
                : detector.CpuUsageThresholdPercentage,
            Timestamp = stats.Timestamp
        };

        await alertService.SendHighUsageAlertAsync(highUsageAlert);
        logger.LogWarning($"High usage alert: {highUsageAlert.MetricType} on {stats.ServerIdentifier}");
    }
}