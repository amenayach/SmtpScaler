namespace SmtpScaler
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.IO;

    public class Config
    {
        public static string ConfigPath => Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

        public static T GetSection<T>(string sectionName)
        {
            if (!File.Exists(ConfigPath))
            {
                throw new ArgumentException("appsettings");
            }

            var json = JObject.Parse(File.ReadAllText(ConfigPath))[sectionName];

            return JsonConvert.DeserializeObject<T>(json.ToString());
        }
    }
}