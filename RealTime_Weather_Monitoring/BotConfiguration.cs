namespace RealTime_Weather_Monitoring;

using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class BotConfiguration
{
    public Dictionary<string, dynamic> Bots { get; set; }

    public static BotConfiguration LoadConfiguration(string filePath)
    {
        var json = File.ReadAllText(filePath);
        var converted = JsonConvert.DeserializeObject<Dictionary<string,dynamic>>(json);
        return new BotConfiguration
        {
            Bots = converted
        };
        }
        
    }

