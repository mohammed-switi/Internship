namespace RealTime_Weather_Monitoring;
public abstract class WeatherBot : IWeatherObserver
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

    public void Update(WeatherData weatherData)
    {
            CheckActivation(weatherData);
    }
    public abstract void CheckActivation(WeatherData data);
}
