using System;
using System.IO;
using Xunit;
using RealTime_Weather_Monitoring;

namespace RealTime_Weather_Monitoring_Test
{
    public class BotConfigurationTests
    {
        [Fact]
        public void LoadConfiguration_ReturnsBotsDictionary_WhenFileExists()
        {
            // Arrange
            string tempConfigPath = Path.GetTempFileName();
            string jsonContent = "{\"Bot1\": {\"url\": \"http://example.com/bot1\"}, \"Bot2\": {\"url\": \"http://example.com/bot2\"}}";
            File.WriteAllText(tempConfigPath, jsonContent);
            
            // Act
            BotConfiguration config = BotConfiguration.LoadConfiguration(tempConfigPath);
            
            // Assert
            Assert.NotNull(config);
            Assert.NotNull(config.Bots);
            Assert.True(config.Bots.Count == 2);
            
            // Cleanup
            File.Delete(tempConfigPath);
        }
    }
}