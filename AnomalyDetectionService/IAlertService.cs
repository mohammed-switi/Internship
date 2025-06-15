
namespace AnomalyDetectionService;

public interface IAlertService
{

    Task SendAnomalyAlertAsync(AnomalyAlert alert);


    Task SendHighUsageAlertAsync(HighUsageAlert alert);
}