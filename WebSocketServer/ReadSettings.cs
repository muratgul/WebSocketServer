using System.Configuration;

namespace WebSocketServerApp
{
    public static class ReadSettings
    {
        public static string FromConfig(string key)
        {
            return ConfigurationManager.AppSettings.Get(key);
        }
    }
}
