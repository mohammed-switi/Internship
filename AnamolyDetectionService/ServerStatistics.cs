namespace AnamolyDetectionService;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

public class ServerStatistics
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("serverIdentifier")]
    public string ServerIdentifier { get; set; } = string.Empty;
    
    
    [BsonElement("memoryUsage")]
    public long MemoryUsage { get; set; } 

    [BsonElement("availableMemory")]
    public long AvailableMemory { get; set; }

    [BsonElement("cpuUsage")]
    public double CpuUsage { get; set; } 


    [BsonElement("timestamp")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime Timestamp { get; set; }
}
