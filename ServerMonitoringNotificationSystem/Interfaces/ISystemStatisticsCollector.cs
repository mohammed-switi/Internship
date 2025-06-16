using ServerMonitoringNotificationSystem.Models;

namespace ServerMonitoringNotificationSystem.Interfaces;
public interface ISystemStatisticsCollector
{
    Task<ServerStatistics> CollectStatisticsAsync(string serverIdentifier);
}