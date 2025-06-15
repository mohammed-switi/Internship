namespace AnomalyDetectionService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

public static class DependencyInjection
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // MongoDB
        services.AddSingleton<IMongoClient>(sp =>
        {
            var mongoConnectionString = configuration.GetConnectionString("MongoDb") ?? "mongodb://localhost:27017";
            return new MongoClient(mongoConnectionString);
        });
        services.AddSingleton<IMongoRepository, MongoRepository>();

        // RabbitMQ
        services.AddSingleton<IMessageConsumer<ServerStatistics>>(sp =>
        {
            var config = configuration.GetSection("RabbitMq").Get<RabbitMqConfig<>>();
            return new RabbitMqConsumer<>(config);
        });

        // Anomaly Detector
        services.AddSingleton<IAnomalyDetector>(sp =>
            new AnomalyDetector(
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
        services.AddHealthChecks()
            .AddMongoDb(configuration.GetConnectionString("MongoDb") ?? "mongodb://localhost:27017", name: "MongoDB");

        return services;
    }
}
