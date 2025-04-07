using System.Collections.Generic;
namespace RealTime_Weather_Monitoring
{
    public class WeatherSubject
    {
        private readonly List<IWeatherObserver> _observers = new List<IWeatherObserver>();

        public void Attach(IWeatherObserver observer)
        {
            _observers.Add(observer);
        }

        public void Detach(IWeatherObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify(WeatherData weatherData)
        {
            foreach (var observer in _observers)
            {
                observer.Update(weatherData);
            }
        }
    }
}