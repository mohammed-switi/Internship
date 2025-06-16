
using AnomalyDetectionService.Models;

namespace AnomalyDetectionService.Interfaces;

public interface IAlertService
{

    Task SendAnomalyAlertAsync(AnomalyAlert alert);


    Task SendHighUsageAlertAsync(HighUsageAlert alert);
}