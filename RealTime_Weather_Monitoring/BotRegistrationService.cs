namespace RealTime_Weather_Monitoring
{
    public class BotRegistrationService
    {
        public void RegisterBots(BotConfiguration config, WeatherSubject weatherSubject)
        {
            if (config.Bots.ContainsKey("RainBot"))
            {
                double humidityThreshold = config.Bots["RainBot"]["humidityThreshold"];
                string message = config.Bots["RainBot"]["message"];
                bool enabled = config.Bots["RainBot"]["enabled"];

                weatherSubject.Attach(new RainBot(humidityThreshold, message, enabled));
            }

            if (config.Bots.ContainsKey("SunBot"))
            {
                double temperatureThreshold = config.Bots["SunBot"]["temperatureThreshold"];
                string message = config.Bots["SunBot"]["message"];
                bool enabled = config.Bots["SunBot"]["enabled"];

                weatherSubject.Attach(new SunBot(temperatureThreshold, message, enabled));
            }

            if (config.Bots.ContainsKey("SnowBot"))
            {
                double temperatureThreshold = config.Bots["SnowBot"]["temperatureThreshold"];
                string message = config.Bots["SnowBot"]["message"];
                bool enabled = config.Bots["SnowBot"]["enabled"];

                weatherSubject.Attach(new SnowBot(temperatureThreshold, message, enabled));
            }
        }
    }
}