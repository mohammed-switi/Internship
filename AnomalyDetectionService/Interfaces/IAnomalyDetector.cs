namespace AnomalyDetectionService.Interfaces;

public interface IAnomalyDetector

{
   
    double MemoryUsageThresholdPercentage { get; }
    double CpuUsageThresholdPercentage { get; }
    
    
     bool DetectMemoryAnomaly(double currentMemoryUsage, double previousMemoryUsage);


     bool DetectCpuAnomaly(double currentCpuUsage, double previousCpuUsage);

  
    bool IsHighUsageMemory(double currentMemoryUsage, double memoryAvailable);

    bool IsHighUsageCpu(double currentCpuUsage);
    
    
    
    
}