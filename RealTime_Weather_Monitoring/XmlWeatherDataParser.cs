using System.Xml.Linq;
namespace RealTime_Weather_Monitoring;
public class XmlWeatherDataParser : IWeatherDataParser
{
    public WeatherData Parse(string data)
    {
        var doc = XDocument.Parse(data);
        if (doc.Root != null)
            return new WeatherData
            {
                Location = doc.Root.Element("Location")!.Value,
                Temperature = double.Parse(doc.Root.Element("Temperature")!.Value),
                Humidity = double.Parse(doc.Root.Element("Humidity")!.Value)
            };
        else
        {
            return null;
        } 
    }
}
