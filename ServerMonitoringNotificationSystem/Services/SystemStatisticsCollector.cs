// Cross-Platform System Statistics Collector

using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using ServerMonitoringNotificationSystem.Interfaces;
using ServerMonitoringNotificationSystem.Models;

namespace ServerMonitoringNotificationSystem.Services;

public class SystemStatisticsCollector : ISystemStatisticsCollector
{
    private readonly ILogger<SystemStatisticsCollector> _logger;
    private PerformanceCounter _cpuCounter = null!;
    private PerformanceCounter _availableMemoryCounter = null!;

    public SystemStatisticsCollector(ILogger<SystemStatisticsCollector> logger)
    {
        _logger = logger;
        _logger.LogInformation("System Statistics Collector initialized. OS: " +
                               (RuntimeInformation.OSDescription ?? "Unknown"));

        InitializePerformanceCountersWindowsOSOnly();
    }

    private bool IsWindowsOs()
    {
        return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
    }

    private void InitializePerformanceCountersWindowsOSOnly()
    {
        if (!IsWindowsOs())
        {
            _logger.LogInformation("Performance counters are only available on Windows. Using cross-platform methods.");
            return;
        }
        // Warning is misleading, the code is not reaching this point on non-Windows OS.
        _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total", true);
        _availableMemoryCounter = new PerformanceCounter("Memory", "Available MBytes");
        _cpuCounter.NextValue();
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

            _logger.LogInformation(
                "Collected statistics - CPU: {Cpu}%, Memory Used: {MemUsed}MB, Available: {MemAvail}MB",
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
    return IsWindowsOs()
        ? await GetCpuUsageWindowsAsync()
        : GetCpuUsageCrossPlatform();
}

private async Task<double> GetCpuUsageWindowsAsync()
{
    try
    {
        await Task.Delay(100);
        // Warning is misleading, the code is not reaching this point on non-Windows OS.
        return Math.Round(_cpuCounter.NextValue(), 2);
    }
    catch (Exception ex)
    {
        _logger.LogWarning(ex, "Failed to get CPU usage from performance counter.");
        return GetCpuUsageCrossPlatform(); // fallback if performance counter fails
    }
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
    return IsWindowsOs() ? GetWindowsMemoryInfo() : GetCrossPlatformMemoryInfo();
}

private (double UsedMemoryMB, double AvailableMemoryMB, double UsagePercent) GetWindowsMemoryInfo()
{
    try
    {
        // Warning is misleading, the code is not reaching this point on non-Windows OS.
        var availableMb = _availableMemoryCounter.NextValue();
        var totalMemoryMb = GetTotalPhysicalMemoryMbWindowsOsOnly();
        var usedMemoryMb = totalMemoryMb - availableMb;
        var usagePercent = usedMemoryMb / totalMemoryMb * 100;

        return (
            UsedMemoryMB: Math.Round(usedMemoryMb, 2),
            AvailableMemoryMB: Math.Round(availableMb, 2),
            UsagePercent: Math.Round(usagePercent, 2)
        );
    }
    catch (Exception ex)
    {
        _logger.LogWarning(ex, "Failed to get memory info from performance counter");
        return GetCrossPlatformMemoryInfo();
    }
}

private (double UsedMemoryMB, double AvailableMemoryMB, double UsagePercent) GetCrossPlatformMemoryInfo()
{
    double usedMb = Environment.WorkingSet / (1024.0 * 1024.0);
    double availableMb = GC.GetTotalMemory(forceFullCollection: false) / (1024.0 * 1024.0);

    return (
        UsedMemoryMB: Math.Round(usedMb, 2),
        AvailableMemoryMB: Math.Round(availableMb, 2),
        UsagePercent: 0.0
    );
}

private double GetTotalPhysicalMemoryMbWindowsOsOnly()
{ 
    // Warning is misleading, the code is not reaching this point on non-Windows OS.
    using var pc = new PerformanceCounter("Memory", "Committed Bytes");
    return pc.NextValue() / (1024.0 * 1024.0);
}

}