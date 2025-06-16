using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AnomalyDetectionService.Models;

public class HighUsageAlert
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("serverIdentifier")]
    public string ServerIdentifier { get; set; } = string.Empty;

    [BsonElement("metricType")]
    public string MetricType { get; set; } = string.Empty; 

    [BsonElement("currentValue")]
    public double CurrentValue { get; set; }

    [BsonElement("threshold")]
    public double Threshold { get; set; }

    [BsonElement("timestamp")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime Timestamp { get; set; }
}