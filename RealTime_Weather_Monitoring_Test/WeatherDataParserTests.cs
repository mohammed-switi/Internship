using Xunit;
using RealTime_Weather_Monitoring; // Assumes WeatherDataParserFactory, IWeatherDataParser, WeatherData exist

namespace RealTime_Weather_Monitoring_Test
{
    public class WeatherDataParserTests
    {
        [Fact]
        public void GetParser_ReturnsJsonParser_ForJsonInput()
        {
            // Arrange
            string jsonInput = "{\"Temperature\":25,\"Humidity\":60}";
            
            // Act
            IWeatherDataParser parser = WeatherDataParserFactory.GetParser(jsonInput);
            
            // Assert
            Assert.NotNull(parser);
            Assert.Equal("JsonWeatherDataParser", parser.GetType().Name);
        }
        
        [Fact]
        public void GetParser_ReturnsXmlParser_ForXmlInput()
        {
            // Arrange
            string xmlInput = "<WeatherData><Temperature>25</Temperature><Humidity>60</Humidity></WeatherData>";
            
            // Act
            IWeatherDataParser parser = WeatherDataParserFactory.GetParser(xmlInput);
            
            // Assert
            Assert.NotNull(parser);
            Assert.Equal("XmlWeatherDataParser", parser.GetType().Name);
        }
        
        [Fact]
        public void Parse_ReturnsValidWeatherData_ForJsonInput()
        {
            // Arrange
            string jsonInput = "{\"Temperature\":25,\"Humidity\":60}";
            IWeatherDataParser parser = WeatherDataParserFactory.GetParser(jsonInput);
            
            // Act
            WeatherData data = parser.Parse(jsonInput);
            
            // Assert
            Assert.NotNull(data);
            Assert.Equal(25, data.Temperature);
            Assert.Equal(60, data.Humidity);
        }
    }
}