using System;
using System.Text.Json.Serialization;

namespace ServerMonitoringNotificationSystem;

[Serializable]
public class ServerStatistics
{
    [JsonPropertyName("serverIdentifier")]
    public required string ServerIdentifier { get; set; }
    
    [JsonPropertyName("memoryUsage")]
    public double MemoryUsage { get; set; }
    
    [JsonPropertyName("availableMemory")]
    public double AvailableMemory { get; set; }
    
    [JsonPropertyName("cpuUsage")]
    public double CpuUsage { get; set; }
    
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }
}