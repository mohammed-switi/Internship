using System;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ServerMonitoringNotificationSystem.Models;

[Serializable]
public class ServerStatistics
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonPropertyName("id")]
    public string? Id { get; set; }


    [BsonElement("serverIdentifier")]
    [JsonPropertyName("serverIdentifier")]
    public string ServerIdentifier { get; set; } = string.Empty;


    [JsonPropertyName("memoryUsage")]
    [BsonElement("memoryUsage")]
    public double MemoryUsage { get; set; }


    [JsonPropertyName("availableMemory")]
    [BsonElement("availableMemory")]
    public double AvailableMemory { get; set; }

    [JsonPropertyName("cpuUsage")]
    [BsonElement("cpuUsage")]
    public double CpuUsage { get; set; }

    [JsonPropertyName("timestamp")]
    [BsonElement("timestamp")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime Timestamp { get; set; }
}