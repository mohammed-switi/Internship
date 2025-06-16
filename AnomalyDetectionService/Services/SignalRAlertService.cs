using AnomalyDetectionService.Interfaces;
using AnomalyDetectionService.Models;

namespace AnomalyDetectionService.Services;

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

public class SignalRAlertService : IAlertService
{
    private readonly HubConnection _hubConnection;


    public SignalRAlertService(string hubUrl)
    {
       var url = hubUrl ?? throw new ArgumentNullException(nameof(hubUrl));

        _hubConnection = BuildHubConnection(url);

        RegisterConnectionClosedHandler();
    }


    private static HubConnection BuildHubConnection(string hubUrl)
    {
        return new HubConnectionBuilder()
            .WithUrl(hubUrl)
            .WithAutomaticReconnect()
            .Build();
    }

    private void RegisterConnectionClosedHandler()
    {
        _hubConnection.Closed += OnHubConnectionOnClosed;
    }

    private async Task OnHubConnectionOnClosed(Exception? error)
    {
        Console.WriteLine("[SignalR] Connection closed. Attempting to reconnect...");
        await Task.Delay(RandomJitterDelay());
        await TryStartConnectionAsync();
    }


    public async Task StartAsync()
    {
        await TryStartConnectionAsync();
    }


    private async Task TryStartConnectionAsync()
    {
        while (!await TryConnectOnceAsync())
        {
            Console.WriteLine("[SignalR] Retrying in 5 seconds...");
            await Task.Delay(5000);
        }
    }

    private async Task<bool> TryConnectOnceAsync()
    {
        try
        {
            await _hubConnection.StartAsync();
            Console.WriteLine("[SignalR] Connected to hub.");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[SignalR] Connection failed: {ex.Message}");
            return false;
        }
    }


    public async Task SendAnomalyAlertAsync(AnomalyAlert anomalyAlert)
    {
        if (!IsConnected())
            await TryStartConnectionAsync();

        await _hubConnection.InvokeAsync("SendAnomalyAlert", anomalyAlert);
    }

    public async Task SendHighUsageAlertAsync(HighUsageAlert highUsageAlert)
    {
        if (!IsConnected())
            await TryStartConnectionAsync();

        await _hubConnection.InvokeAsync("SendHighUsageAlert", highUsageAlert);
    }

    private bool IsConnected()
    {
        return _hubConnection.State == HubConnectionState.Connected;
    }
    private static int RandomJitterDelay()
    {
        var rnd = new Random();
        return rnd.Next(1000, 5000);
    }
}