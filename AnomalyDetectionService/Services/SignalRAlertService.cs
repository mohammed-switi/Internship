using AnomalyDetectionService.Interfaces;
using AnomalyDetectionService.Models;

namespace AnomalyDetectionService;

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

public class SignalRAlertService : IAlertService
{
    private readonly HubConnection _hubConnection;
    private readonly string _hubUrl;

    public SignalRAlertService(string hubUrl)
    {
        _hubUrl = hubUrl ?? throw new ArgumentNullException(nameof(hubUrl));

        _hubConnection = new HubConnectionBuilder()
            .WithUrl(_hubUrl)
            .WithAutomaticReconnect() 
            .Build();

        _hubConnection.Closed += async (error) =>
        {
            Console.WriteLine("[SignalR] Connection closed. Attempting to reconnect...");
            await Task.Delay(RandomJitterDelay());
            await TryStartConnectionAsync();
        };
    }

    public async Task StartAsync()
    {
        await TryStartConnectionAsync();
    }

    private async Task TryStartConnectionAsync()
    {
        while (true)
        {
            try
            {
                await _hubConnection.StartAsync();
                Console.WriteLine("[SignalR] Connected to hub.");
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SignalR] Connection failed: {ex.Message}. Retrying in 5 seconds...");
                await Task.Delay(5000);
            }
        }
    }

    public async Task SendAnomalyAlertAsync(AnomalyAlert anomalyAlert)
    {
        if (_hubConnection.State != HubConnectionState.Connected)
            await TryStartConnectionAsync();

        await _hubConnection.InvokeAsync("SendAnomalyAlert", anomalyAlert);
    }

    public async Task SendHighUsageAlertAsync(HighUsageAlert highUsageAlert)
    {
        if (_hubConnection.State != HubConnectionState.Connected)
            await TryStartConnectionAsync();

        await _hubConnection.InvokeAsync("SendHighUsageAlert", highUsageAlert);
    }

    private static int RandomJitterDelay()
    {
        var rnd = new Random();
        return rnd.Next(1000, 5000);
    }

}
