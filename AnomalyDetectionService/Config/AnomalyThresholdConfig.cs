namespace AnomalyDetectionService.Config;

public class AnomalyThresholdConfig
{
    public double MemoryUsageThresholdPercentage { get; } = 0.8; // 80% threshold
    public double CpuUsageThresholdPercentage { get; } = 0.8; // 80% threshold
    public double MemoryUsageAnomalyThresholdPercentage { get; } = 0.2; // 20% anomaly threshold
    public double CpuUsageAnomalyThresholdPercentage { get; } = 0.2; // 20% anomaly threshold
}