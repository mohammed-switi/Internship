// C#
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using AnomalyDetectionService.Config;
using AnomalyDetectionService.Consumers;
using AnomalyDetectionService.Interfaces;
using AnomalyDetectionService.Models;
using AnomalyDetectionService.Repositories;
using AnomalyDetectionService.Services;
using ServerMonitoringNotificationSystem.Models;

namespace AnomalyDetectionService;

public static class DependencyInjection
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure MongoDbConfig
        services.Configure<MongoDbConfig>(configuration.GetSection("MongoDb"));

        // MongoDB
        services.AddSingleton<IMongoClient>(sp =>
        {
            var mongoOptions = sp.GetRequiredService<IOptions<MongoDbConfig>>().Value;
            return new MongoClient(mongoOptions.ConnectionString);
        });
        services.AddSingleton<IMongoRepository>(sp =>
        {
            var mongoClient = sp.GetRequiredService<IMongoClient>();
            var mongoOptions = sp.GetRequiredService<IOptions<MongoDbConfig>>().Value;
            return new MongoRepository(mongoClient, mongoOptions.DatabaseName);
        });

        // RabbitMQ
        services.AddSingleton<IMessageConsumer<ServerStatistics>>(sp =>
        {
            var config = configuration.GetSection("RabbitMq").Get<RabbitMqConfig>();
            ArgumentNullException.ThrowIfNull(config);
            
            var logger = sp.GetRequiredService<ILogger<RabbitMqConsumer<ServerStatistics>>>();
          
            return new RabbitMqConsumer<ServerStatistics>(
                config.HostName,
                config.ExchangeName,
                logger,
                routingKeyPattern: config.RoutingKey
            );
        });

        // Anomaly Detector
        services.AddSingleton<IAnomalyDetector>(sp =>
            new AnomalyDetectorService(
                memoryUsageAnomalyThresholdPercentage: 0.3,
                cpuUsageAnomalyThresholdPercentage: 0.3,
                memoryUsageThresholdPercentage: 0.85,
                cpuUsageThresholdPercentage: 0.85
            )
        );

        // SignalR
        services.AddSingleton<IAlertService>(sp =>
        {
            var config = configuration.GetSection("SignalR").Get<SignalRConfig>();
            ArgumentNullException.ThrowIfNull(config);
            var alertService = new SignalRAlertService(config.HubUrl);
            alertService.StartAsync().GetAwaiter().GetResult();
            return alertService;
        });

        // Hosted Service
        services.AddHostedService<StatisticsProcessingService>();

        // Logging
        services.AddLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
        });

  
        return services;
    }
}