using RealTime_Weather_Monitoring;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.WriteLine("Loading configuration...");

        string configPath = "/home/sowaity/RiderProjects/Internship/RealTime_Weather_Monitoring/config.json";
        var config = BotConfiguration.LoadConfiguration(configPath);
        Console.WriteLine(config);

        var weatherSubject = new WeatherSubject();
        var botRegistrationService = new BotRegistrationService();
        botRegistrationService.RegisterBots(config, weatherSubject);

        Console.WriteLine("Enter weather data (JSON or XML):");

        while (true)
        {
            string input = Console.ReadLine();

            try
            {
                IWeatherDataParser parser = WeatherDataParserFactory.GetParser(input);
                WeatherData weatherData = parser.Parse(input);
                weatherSubject.Notify(weatherData);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Error processing data: {ex.Message}");
            }
        }
    }
}