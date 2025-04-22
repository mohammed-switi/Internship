 using System.Collections.Generic;
 using Xunit;
 using RealTime_Weather_Monitoring;
 
 namespace RealTime_Weather_Monitoring_Test
 {
     // A dummy bot that implements IWeatherObserver.
     public class DummyBot : IWeatherObserver
     {
         public WeatherData LastNotifiedData { get; private set; }
 
         public void Update(WeatherData data)
         {
             LastNotifiedData = data;
         }
     }
 
     public class BotRegistrationServiceTests
     {
         [Fact]
         public void RegisterBots_AttachesBotsToWeatherSubject()
         {
             // Arrange: include all required properties for each bot using Dictionary<string, object>.
             BotConfiguration config = new BotConfiguration
             {
                 Bots = new Dictionary<string, Dictionary<string, object>>
                 {
                     { "RainBot", new Dictionary<string, object>
                         {
                             { "url", "http://example.com/botA" },
                             { "humidityThreshold", 80.0 },
                             { "message", "It is raining" },
                             { "enabled", true }
                         }
                     },
                     { "SnowBot", new Dictionary<string, object>
                         {
                             { "url", "http://example.com/botB" },
                             { "temperatureThreshold", -5.0 },
                             { "message", "It is snowing" },
                             { "enabled", true }
                         }
                     }
                 }
             };
 
             WeatherSubject subject = new WeatherSubject();
             BotRegistrationService registrationService = new BotRegistrationService();
 
             // Act
             registrationService.RegisterBots(config, subject);
 
             // Assert
             var observersCount = subject.GetObserversCount();
             Assert.Equal(2, observersCount);
         }
     }
}