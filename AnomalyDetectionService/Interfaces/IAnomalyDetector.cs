namespace AnomalyDetectionService;

public interface IAnomalyDetector
{

    public bool DetectMemoryAnomaly(double currentMemoryUsage, double previousMemoryUsage);


    public bool DetectCpuAnomaly(double currentCpuUsage, double previousCpuUsage);

  
    bool IsHighUsageMemory(double currentMemoryUsage, double memoryAvailable);

    bool IsHighUsageCpu(double currentCpuUsage);
}