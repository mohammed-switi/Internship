namespace RealTime_Weather_Monitoring;

using System;

public abstract class WeatherBot
{
    public string Name { get; }
    public string Message { get; }
    public bool IsEnabled { get; }

    protected WeatherBot(string name, string message, bool isEnabled)
    {
        Name = name;
        Message = message;
        IsEnabled = isEnabled;
    }

    public abstract void CheckActivation(WeatherData data);
}

public class RainBot : WeatherBot
{
    private double HumidityThreshold;

    public RainBot(double threshold, string message, bool isEnabled)
        : base("RainBot", message, isEnabled)
    {
        HumidityThreshold = threshold;
    }

    public override void CheckActivation(WeatherData data)
    {
        if (IsEnabled && data.Humidity > HumidityThreshold)
        {
            Console.WriteLine($"{Name} activated!\n{Message}");
        }
    }
}

public class SunBot : WeatherBot
{
    private double TemperatureThreshold;

    public SunBot(double threshold, string message, bool isEnabled)
        : base("SunBot", message, isEnabled)
    {
        TemperatureThreshold = threshold;
    }

    public override void CheckActivation(WeatherData data)
    {
        if (IsEnabled && data.Temperature > TemperatureThreshold)
        {
            Console.WriteLine($"{Name} activated!\n{Message}");
        }
    }
}

public class SnowBot : WeatherBot
{
    private double TemperatureThreshold;

    public SnowBot(double threshold, string message, bool isEnabled)
        : base("SnowBot", message, isEnabled)
    {
        TemperatureThreshold = threshold;
    }

    public override void CheckActivation(WeatherData data)
    {
        if (IsEnabled && data.Temperature < TemperatureThreshold)
        {
            Console.WriteLine($"{Name} activated!\n{Message}");
        }
    }
}
