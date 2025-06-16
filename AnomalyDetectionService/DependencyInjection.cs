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
            if(config==null) throw new ArgumentNullException(paramName:nameof(configuration),message:"RabbitMq configuration is missing.");
            var logger = sp.GetRequiredService<ILogger<RabbitMqConsumer<ServerStatistics>>>();
            logger.LogInformation("something "  + config.ExchangeName);
          
            return new RabbitMqConsumer<ServerStatistics>(
                config.HostName,
                config.ExchangeName
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
            var hubUrl = configuration["SignalR:HubUrl"] ?? "https://localhost:5001/alertHub";
            var alertService = new SignalRAlertService(hubUrl);
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

        // Health Checks
  
        return services;
    }
}