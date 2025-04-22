using Newtonsoft.Json;
namespace RealTime_Weather_Monitoring;
public class JsonWeatherDataParser : IWeatherDataParser
{
    public WeatherData Parse(string data)
    {
        return JsonConvert.DeserializeObject<WeatherData>(data);
    }
}