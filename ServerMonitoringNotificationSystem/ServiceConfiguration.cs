using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ServerMonitoringNotificationSystem.Config;
using ServerMonitoringNotificationSystem.Interfaces;
using ServerMonitoringNotificationSystem.Publisher;
using ServerMonitoringNotificationSystem.Services;

namespace ServerMonitoringNotificationSystem
{
    public static class ServiceConfiguration
    {
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var config = new MonitoringConfiguration();
                    hostContext.Configuration.GetSection("Monitoring").Bind(config);
                    services.AddSingleton(config);

                    services.AddSingleton<ISystemStatisticsCollector>(sp =>
                    {
                        var logger = sp.GetRequiredService<ILogger<SystemStatisticsCollector>>();
                        return new SystemStatisticsCollector(logger );
                    });

                    services.AddSingleton<IMessageQueuePublisher, RabbitMqPublisher>(
                        sp =>
                        {
                            var monitoringConfig = sp.GetRequiredService<MonitoringConfiguration>();
                            ArgumentNullException.ThrowIfNull(monitoringConfig);
                            var logger = sp.GetRequiredService<ILogger<RabbitMqPublisher>>();
                            return new RabbitMqPublisher(monitoringConfig.MessageQueueConnectionString, logger);
                        }
                        );

                    services.AddSingleton<ServerStatisticsCollectionService>(sp =>
                    {
                        return new ServerStatisticsCollectionService(
                            sp.GetRequiredService<ISystemStatisticsCollector>(),
                            sp.GetRequiredService<IMessageQueuePublisher>(),
                            sp.GetRequiredService<MonitoringConfiguration>(),
                            sp.GetRequiredService<ILogger<ServerStatisticsCollectionService>>()
                        );
                    });

                    services.AddHostedService<ServerStatisticsCollectionService>(sp =>
                        sp.GetRequiredService<ServerStatisticsCollectionService>()
                    );
                });

             
        }
    }
}