namespace RealTime_Weather_Monitoring;

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
