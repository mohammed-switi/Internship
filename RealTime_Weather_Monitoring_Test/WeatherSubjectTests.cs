using System.Collections.Generic;
using Xunit;
using RealTime_Weather_Monitoring;

namespace RealTime_Weather_Monitoring_Test
{
    public class DummyWeatherObserver : IWeatherObserver
    {
        public List<WeatherData> ReceivedData { get; } = new List<WeatherData>();
        
        public void Update(WeatherData data)
        {
            ReceivedData.Add(data);
        }
    }
    
    public class WeatherSubjectTests
    {
        [Fact]
        public void Notify_CallsObserverUpdate_WithCorrectWeatherData()
        {
            WeatherSubject subject = new WeatherSubject();
            DummyWeatherObserver observer = new DummyWeatherObserver();
            
            subject.Attach(observer);
            
            WeatherData sampleData = new WeatherData { Temperature = 30, Humidity = 50 };
            
            subject.Notify(sampleData);
            
            Assert.Single(observer.ReceivedData);
            Assert.Equal(30, observer.ReceivedData[0].Temperature);
            Assert.Equal(50, observer.ReceivedData[0].Humidity);
        }
    }
}