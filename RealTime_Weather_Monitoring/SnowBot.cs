namespace RealTime_Weather_Monitoring;
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
