using System;

namespace RealTime_Weather_Monitoring
{
    public class BotRegistrationService
    {
        public void RegisterBots(BotConfiguration config, WeatherSubject weatherSubject)
        {
            if (config.Bots.ContainsKey("RainBot"))
            {
                double humidityThreshold = Convert.ToDouble(config.Bots["RainBot"]["humidityThreshold"]);
                string message = Convert.ToString(config.Bots["RainBot"]["message"]);
                bool enabled = Convert.ToBoolean(config.Bots["RainBot"]["enabled"]);

                weatherSubject.Attach(new RainBot(humidityThreshold, message, enabled));
            }

            if (config.Bots.ContainsKey("SunBot"))
            {
                double temperatureThreshold = Convert.ToDouble(config.Bots["SunBot"]["temperatureThreshold"]);
                string message = Convert.ToString(config.Bots["SunBot"]["message"]);
                bool enabled = Convert.ToBoolean(config.Bots["SunBot"]["enabled"]);

                weatherSubject.Attach(new SunBot(temperatureThreshold, message, enabled));
            }

            if (config.Bots.ContainsKey("SnowBot"))
            {
                double temperatureThreshold = Convert.ToDouble(config.Bots["SnowBot"]["temperatureThreshold"]);
                string message = Convert.ToString(config.Bots["SnowBot"]["message"]);
                bool enabled = Convert.ToBoolean(config.Bots["SnowBot"]["enabled"]);

                weatherSubject.Attach(new SnowBot(temperatureThreshold, message, enabled));
            }
        }
    }
}