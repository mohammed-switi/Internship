
namespace RealTime_Weather_Monitoring
{
    public interface IWeatherObserver
    {
        void Update(WeatherData weatherData);
    }
}