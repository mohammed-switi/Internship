using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AnomalyDetectionService.Models;

public class AnomalyAlert
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("serverIdentifier")]
    public string ServerIdentifier { get; set; } = string.Empty;

    [BsonElement("metricType")]
    public string MetricType { get; set; } = string.Empty; // e.g., "CPU", "Memory"

    [BsonElement("currentValue")]
    public double CurrentValue { get; set; }

    [BsonElement("previousValue")]
    public double PreviousValue { get; set; }

    [BsonElement("timestamp")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime Timestamp { get; set; }
}