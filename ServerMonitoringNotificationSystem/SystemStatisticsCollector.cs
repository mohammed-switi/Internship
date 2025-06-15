// Cross-Platform System Statistics Collector

using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;

namespace ServerMonitoringNotificationSystem;

public interface ISystemStatisticsCollector
{
    Task<ServerStatistics> CollectStatisticsAsync(string serverIdentifier);
}

public class SystemStatisticsCollector : ISystemStatisticsCollector
{
    private readonly ILogger<SystemStatisticsCollector> _logger;
    private PerformanceCounter _cpuCounter;
    private PerformanceCounter _availableMemoryCounter;
    private readonly bool _isWindows;

    public SystemStatisticsCollector(ILogger<SystemStatisticsCollector> logger, PerformanceCounter cpuCounter,
        PerformanceCounter availableMemoryCounter)
    {
        _logger = logger;
        _cpuCounter = cpuCounter;
        _availableMemoryCounter = availableMemoryCounter;
        _isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        InitializePerformanceCounters();
    }

    private void InitializePerformanceCounters()
    {
        try
        {
            if (_isWindows)
            {
                _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total", true);
                _availableMemoryCounter = new PerformanceCounter("Memory", "Available MBytes");

                _cpuCounter.NextValue();
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to initialize performance counters. Using alternative methods.");
        }
    }

    public async Task<ServerStatistics> CollectStatisticsAsync(string serverIdentifier)
    {
        var statistics = new ServerStatistics
        {
            ServerIdentifier = serverIdentifier,
            Timestamp = DateTime.UtcNow
        };

        try
        {
            statistics.CpuUsage = await GetCpuUsageAsync();

            var memoryInfo = GetMemoryInfo();
            statistics.MemoryUsage = memoryInfo.UsedMemoryMB;
            statistics.AvailableMemory = memoryInfo.AvailableMemoryMB;

            _logger.LogDebug("Collected statistics - CPU: {Cpu}%, Memory Used: {MemUsed}MB, Available: {MemAvail}MB",
                statistics.CpuUsage, statistics.MemoryUsage, statistics.AvailableMemory);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error collecting system statistics");
        }

        return statistics;
    }

    private async Task<double> GetCpuUsageAsync()
    {
        if (_isWindows && _cpuCounter != null)
            try
            {
                // Wait a bit for accurate reading
                await Task.Delay(100);
                return Math.Round(_cpuCounter.NextValue(), 2);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to get CPU usage from performance counter");
            }

        return GetCpuUsageCrossPlatform();
    }

    private double GetCpuUsageCrossPlatform()
    {
        try
        {
            var startTime = DateTime.UtcNow;
            var startCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;

            Thread.Sleep(500);

            var endTime = DateTime.UtcNow;
            var endCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;

            var cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
            var totalMsPassed = (endTime - startTime).TotalMilliseconds;
            var cpuUsageTotal = cpuUsedMs / (Environment.ProcessorCount * totalMsPassed);

            return Math.Round(cpuUsageTotal * 100, 2);
        }
        catch
        {
            return 0.0;
        }
    }

    private (double UsedMemoryMB, double AvailableMemoryMB, double UsagePercent) GetMemoryInfo()
    {
        try
        {
            if (_isWindows)
            {
                var availableMb = _availableMemoryCounter.NextValue();
                var totalMemoryMb = GetTotalPhysicalMemoryMB();
                var usedMemoryMb = totalMemoryMb - availableMb;
                var usagePercent = usedMemoryMb / totalMemoryMb * 100;

                return (Math.Round(usedMemoryMb, 2), Math.Round(availableMb, 2), Math.Round(usagePercent, 2));
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to get memory info from performance counter");
        }

        var gcMemoryMb = GC.GetTotalMemory(false) / (1024.0 * 1024.0);
        var workingSetMb = Environment.WorkingSet / (1024.0 * 1024.0);

        return (Math.Round(workingSetMb, 2), Math.Round(gcMemoryMb, 2), 0.0);
    }

    private double GetTotalPhysicalMemoryMB()
    {
        using var pc = new PerformanceCounter("Memory", "Committed Bytes");
        return pc.NextValue() / (1024.0 * 1024.0);
    }
}