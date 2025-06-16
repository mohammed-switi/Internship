using Microsoft.AspNetCore.SignalR;
using AnomalyDetectionService.Models; // Adjust the namespace as needed
var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:3000", "http://localhost:5173") // Add your client URLs
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });
});

var app = builder.Build();

// Configure pipeline
app.UseCors();
app.UseRouting();


app.MapHub<AlertHub>("/alertHub");
// Optional: Add a simple endpoint to test if server is running
app.MapGet("/", () => "SignalR Hub Server is running!");

app.Run("http://localhost:5001"); // Explicitly set to port 5001
public class AlertHub : Hub
{
    public async Task SendAnomalyAlert(AnomalyAlert anomalyAlert)
    {
        // Broadcast to all connected clients
        await Clients.All.SendAsync("ReceiveAnomalyAlert", anomalyAlert);
        Console.WriteLine($"[Hub] Sent anomaly alert: {anomalyAlert}");
    }

    public async Task SendHighUsageAlert(HighUsageAlert highUsageAlert)
    {
        // Broadcast to all connected clients
        await Clients.All.SendAsync("ReceiveHighUsageAlert", highUsageAlert);
        Console.WriteLine($"[Hub] Sent high usage alert: {highUsageAlert}");
    }

    public override async Task OnConnectedAsync()
    {
        Console.WriteLine($"[Hub] Client connected: {Context.ConnectionId}");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        Console.WriteLine($"[Hub] Client disconnected: {Context.ConnectionId}");
        await base.OnDisconnectedAsync(exception);
    }
}
