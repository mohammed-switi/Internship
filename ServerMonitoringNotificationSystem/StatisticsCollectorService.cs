using System.Diagnostics;

namespace ServerMonitoringNotificationSystem;

public class StatisticsCollectorService
{
    private readonly string _serverIdentifier;
    private readonly int _intervalSeconds;
    private readonly IMessageQueuePublisher _publisher;
    private readonly PerformanceCounter _cpuCounter;
    private readonly PerformanceCounter _availableMemoryCounter;

    public StatisticsCollectorService(string serverIdentifier, int intervalSeconds, IMessageQueuePublisher publisher)
    {
        _serverIdentifier = serverIdentifier;
        _intervalSeconds = intervalSeconds;
        _publisher = publisher;

        _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        _availableMemoryCounter = new PerformanceCounter("Memory", "Available MBytes");
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var stats = Collect();
            _publisher.PublishAsync($"ServerStatistics.{_serverIdentifier}", stats);
            await Task.Delay(_intervalSeconds * 1000, cancellationToken);
        }
    }

    private ServerStatistics Collect()
    {
        float cpu = _cpuCounter.NextValue();
        float availableMem = _availableMemoryCounter.NextValue();
        var totalMem = GetTotalMemoryInMB();

        return new ServerStatistics
        {
            Timestamp = DateTime.UtcNow,
           
        };
    }

    private float GetTotalMemoryInMB()
    {
        return new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory / (1024 * 1024);
    }
}