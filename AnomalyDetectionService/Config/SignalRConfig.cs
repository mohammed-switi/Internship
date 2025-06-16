namespace AnomalyDetectionService.Config;

public class SignalRConfig
{
    
    public string HubUrl { get; set; } = "http://localhost:5000/alertHub";
}