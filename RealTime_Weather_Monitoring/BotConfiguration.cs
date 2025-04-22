using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace RealTime_Weather_Monitoring
{
    public class BotConfiguration
    {
        public Dictionary<string, Dictionary<string, object>> Bots { get; set; } = new Dictionary<string, Dictionary<string, object>>();

        public static BotConfiguration LoadConfiguration(string filePath)
        {
            string json = File.ReadAllText(filePath);
            var bots = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(json);
            return new BotConfiguration { Bots = bots };
        }
    }
}