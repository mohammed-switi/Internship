namespace AnomalyDetectionService;

public class AnomalyDetectorService(
    double memoryUsageAnomalyThresholdPercentage,
    double cpuUsageAnomalyThresholdPercentage,
    double memoryUsageThresholdPercentage,
    double cpuUsageThresholdPercentage)
    : IAnomalyDetector
{
 

    // Anomaly alert: current > previous * (1 + threshold)
    public bool DetectMemoryAnomaly(double currentMemoryUsage, double previousMemoryUsage)
    {
        return currentMemoryUsage > previousMemoryUsage * (1 + memoryUsageAnomalyThresholdPercentage);
    }

    public bool DetectCpuAnomaly(double currentCpuUsage, double previousCpuUsage)
    {
        return currentCpuUsage > previousCpuUsage * (1 + cpuUsageAnomalyThresholdPercentage);
    }

    public bool IsHighUsageMemory(double currentMemoryUsage, double currentAvailableMemory)
    {
        double usageRatio = currentMemoryUsage / (currentMemoryUsage + currentAvailableMemory);
        return usageRatio > memoryUsageThresholdPercentage;
    }

    public bool IsHighUsageCpu(double currentCpuUsage)
    {
        return currentCpuUsage > cpuUsageThresholdPercentage;
    }


   
}