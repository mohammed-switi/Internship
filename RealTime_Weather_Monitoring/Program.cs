// See https://aka.ms/new-console-template for more information


using RealTime_Weather_Monitoring;

class Program
{
    static void Main()
    {
        Console.WriteLine("Loading configuration...");


        string configPath = "/home/sowaity/RiderProjects/Internship/RealTime_Weather_Monitoring/config.json"; 
        
        var config = BotConfiguration.LoadConfiguration(configPath);
        
        Console.WriteLine(config);
       
        var bots = new List<WeatherBot>();
        

        
            if (config.Bots.ContainsKey("RainBot"))
            {
                double humidityThreshold = config.Bots["RainBot"]["humidityThreshold"];
                string message = config.Bots["RainBot"]["message"];
                bool enabled = config.Bots["RainBot"]["enabled"];
                
                bots.Add(new RainBot(
                    humidityThreshold,
                    message,
                    enabled
                    
                ));
            }

            if (config.Bots.ContainsKey("SunBot"))
            {
                double temperatureThreshold = config.Bots["SunBot"]["temperatureThreshold"];
                string message = config.Bots["SunBot"]["message"];
                bool enabled = config.Bots["SunBot"]["enabled"];
                
                bots.Add(new SunBot(
                    temperatureThreshold,
                    message,
                    enabled
                    
                ));
            }

            if (config.Bots.ContainsKey("SnowBot"))
            {
                double temperatureThreshold = config.Bots["SnowBot"]["temperatureThreshold"];
                string message = config.Bots["SnowBot"]["message"];
                bool enabled = config.Bots["SnowBot"]["enabled"];
                
                bots.Add(new SnowBot(
                    temperatureThreshold,
                    message,
                    enabled
                ));
            }
        
    

        Console.WriteLine("Enter weather data (JSON or XML):");

        while (true)
        {
            string input = Console.ReadLine();

            IWeatherDataParser parser = null;
            if (input.TrimStart().StartsWith("{"))
                parser = new JsonWeatherDataParser();
            else if (input.TrimStart().StartsWith("<"))
                parser = new XmlWeatherDataParser();

            if (parser != null)
            {
                try
                {
                    WeatherData weatherData = parser.Parse(input);

                    foreach (var bot in bots)
                    {
                        bot.CheckActivation(weatherData);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing data: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Unsupported format. Please enter JSON or XML.");
            }
        }
    }
}