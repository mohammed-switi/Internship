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

public class StatisticsProcessingService : BackgroundService
{
    private readonly IMongoRepository _repository;
    private readonly IMessageConsumer<ServerStatistics> _consumer;
    private readonly IAnomalyDetector _detector;
    private readonly IAlertService _alertService;
    private readonly ILogger<StatisticsProcessingService> _logger;

    public StatisticsProcessingService(
        IMongoRepository repository,
        IMessageConsumer<ServerStatistics> consumer,
        IAnomalyDetector detector,
        IAlertService alertService,
        ILogger<StatisticsProcessingService> logger)
    {
        _repository = repository;
        _consumer = consumer;
        _detector = detector;
        _alertService = alertService;
        _logger = logger;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("StatisticsProcessingService is starting.");
        _consumer.onMessageReceived += async (sender, stats) =>
        {
            try
            {
                _logger.LogInformation("Processing statistics message...");
                _logger.LogInformation("Received stats: {Stats}", JsonSerializer.Serialize(stats));
                await HandleStatisticsAsync(stats);
                _logger.LogInformation("Successfully processed statistics message");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing statistics message");
                throw;
            }
        };

        return _consumer.StartConsumingAsync();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.CompletedTask;
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _consumer.StopConsumingAsync();
        _logger.LogInformation("StatisticsProcessingService is stopping.");
        return base.StopAsync(cancellationToken);
    }

    private async Task HandleStatisticsAsync(ServerStatistics stats)
    {
        _logger.LogInformation($"Received stats from {stats.ServerIdentifier} at {stats.Timestamp}");

        await _repository.InsertAsync<ServerStatistics>(stats);

        var history = await _repository.GetRecentAsync<ServerStatistics>(stats.ServerIdentifier, 10);
        var ordered = history.OrderByDescending(s => s.Timestamp).ToList();

        if (ordered.Count < 2)
        {
            _logger.LogInformation("Not enough historical data for anomaly detection.");
            return;
        }

        var previous = ordered[1];

        var memAnomaly = _detector.DetectMemoryAnomaly(stats.MemoryUsage, previous.MemoryUsage);
        var cpuAnomaly = _detector.DetectCpuAnomaly(stats.CpuUsage, previous.CpuUsage);

        if (memAnomaly || cpuAnomaly)
        {
            var anomalyAlert = new AnomalyAlert
            {
                ServerIdentifier = stats.ServerIdentifier,
                MetricType = memAnomaly ? "Memory" : "CPU",
                CurrentValue = memAnomaly ? stats.MemoryUsage : stats.CpuUsage,
                PreviousValue = memAnomaly ? previous.MemoryUsage : previous.CpuUsage,
                Timestamp = stats.Timestamp
            };

            await _alertService.SendAnomalyAlertAsync(anomalyAlert);
            _logger.LogWarning($"Anomaly detected: {anomalyAlert.MetricType} on {stats.ServerIdentifier}");
        }

        // Step 4: High Usage Detection
        var highMemUsage = _detector.IsHighUsageMemory(stats.MemoryUsage, stats.AvailableMemory);
        var highCpuUsage = _detector.IsHighUsageCpu(stats.CpuUsage);

        if (highMemUsage || highCpuUsage)
        {
            var highUsageAlert = new HighUsageAlert
            {
                ServerIdentifier = stats.ServerIdentifier,
                MetricType = highMemUsage ? "Memory" : "CPU",
                CurrentValue = highMemUsage ? stats.MemoryUsage : stats.CpuUsage,
                Threshold = highMemUsage
                    ? _detector.MemoryUsageThresholdPercentage
                    : _detector.CpuUsageThresholdPercentage,
                Timestamp = stats.Timestamp
            };

            await _alertService.SendHighUsageAlertAsync(highUsageAlert);
            _logger.LogWarning($"High usage alert: {highUsageAlert.MetricType} on {stats.ServerIdentifier}");
        }
    }
}