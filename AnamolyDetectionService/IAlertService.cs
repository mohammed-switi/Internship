namespace AnamolyDetectionService;
using System.Threading.Tasks;

public interface IAlertService
{

    Task SendAnomalyAlertAsync(AnomalyAlert alert);


    Task SendHighUsageAlertAsync(HighUsageAlert alert);
}
