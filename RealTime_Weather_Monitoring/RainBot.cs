namespace RealTime_Weather_Monitoring;
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
