namespace RealTime_Weather_Monitoring
{
    public static class WeatherDataParserFactory
    {
        public static IWeatherDataParser GetParser(string input)
        {
            if (input.TrimStart().StartsWith("{"))
            {
                return new JsonWeatherDataParser();
            }
            else if (input.TrimStart().StartsWith("<"))
            {
                return new XmlWeatherDataParser();
            }
            else
            {
                throw new ArgumentException("Unsupported format. Please use JSON or XML.");
            }
        }
    }
}