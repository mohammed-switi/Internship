using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ServerMonitoringNotificationSystem;

public class Program
{
    public static Task Main(string[] args)
    {
     var host = ServiceConfiguration.CreateHostBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .Build();

        Console.WriteLine("=== SERVER MONITORING SYSTEM STARTED ===");
        Console.WriteLine("Press Ctrl+C to stop the service");
        Console.WriteLine();

       return   host.RunAsync();
    }
}