using AnomalyDetectionService.Models;
using Microsoft.AspNetCore.SignalR.Client;

var hubUrl = "http://localhost:5001/alertHub";

var connection = new HubConnectionBuilder()
    .WithUrl(hubUrl)
    .WithAutomaticReconnect()
    .Build();

connection.On<AnomalyAlert>("ReceiveAnomalyAlert", alert =>
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"\n🔴 Anomaly Alert!\nServer: {alert.ServerIdentifier}\nMetric: {alert.MetricType}\nCurrent: {alert.CurrentValue}\nPrevious: {alert.PreviousValue}\nTimestamp: {alert.Timestamp}");
    Console.ResetColor();
});

connection.On<HighUsageAlert>("ReceiveHighUsageAlert", alert =>
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"\n⚠️ High Usage Alert!\nServer: {alert.ServerIdentifier}\nMetric: {alert.MetricType}\nValue: {alert.CurrentValue}\nTimestamp: {alert.Timestamp}");
    Console.ResetColor();
});

try
{
    await connection.StartAsync();
    Console.WriteLine("✅ Connected to SignalR hub.");

    Console.WriteLine("Listening for alerts... Press Ctrl+C to exit.");
    await Task.Delay(-1); // Keeps the app running
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Failed to connect: {ex.Message}");
}
