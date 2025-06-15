// File: ServerMonitoringNotificationSystem/ServiceConfiguration.cs
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ServerMonitoringNotificationSystem
{
    public static class ServiceConfiguration
    {
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    // Bind configuration section if needed
                    var config = new MonitoringConfiguration();
                    hostContext.Configuration.GetSection("Monitoring").Bind(config);
                    services.AddSingleton(config);

                    // Register ISystemStatisticsCollector using a factory to create PerformanceCounter instances.
                    services.AddSingleton<ISystemStatisticsCollector>(sp =>
                    {
                        var logger = sp.GetRequiredService<Microsoft.Extensions.Logging.ILogger<SystemStatisticsCollector>>();
                        return new SystemStatisticsCollector(logger );
                    });

                    // Register the RabbitMQ publisher
                    services.AddSingleton<IMessageQueuePublisher, RabbitMqPublisher>(
                        sp =>
                        {
                            var config = sp.GetRequiredService<MonitoringConfiguration>();
                            var logger = sp.GetRequiredService<Microsoft.Extensions.Logging.ILogger<RabbitMqPublisher>>();
                            return new RabbitMqPublisher(config.MessageQueueConnectionString, logger);
                        }
                        );

                    // Register the hosted service
                    services.AddHostedService<ServerStatisticsCollectionService>();
                });
        }
    }
}