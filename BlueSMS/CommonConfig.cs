using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueSMS
{
    public class CommonConfig
    {
        private static NameValueCollection appSettings = ConfigurationManager.AppSettings;
        private static ConnectionStringSettingsCollection connStrings = ConfigurationManager.ConnectionStrings;
        private const string EnvironmentDefaultKey = "Environment";
        private static string HostName = System.Environment.MachineName;


        // ================== Environment =========================
        /// <summary>
        /// Environment can be specified two ways, by using a combination of HostName and environemnt  e.g. "Environment.HostName" or using the Key "Environment".
        /// If a combination of HostName and Environment is configured for the server that is running the endpoint then this setting will be used in preference.
        /// </summary>
        /// <returns></returns>
        private static string GetEnvironmentKey()
        {
            var hostSpecificKey = EnvironmentDefaultKey + "." + HostName;

            if (appSettings[hostSpecificKey] != null)
            {
                Console.WriteLine("Config is using Host Specific Environmnt Key: {0} Environment: {1}", hostSpecificKey, appSettings[hostSpecificKey].ToUpper());
                return hostSpecificKey;
            }

            Console.WriteLine("Config is using Default Environment key: {0}, Environment: {1}", EnvironmentDefaultKey, appSettings[EnvironmentDefaultKey].ToUpper());
            return EnvironmentDefaultKey;
        }

        public static string GetEnvironment()
        {
            return appSettings[GetEnvironmentKey()];
        }

        public static string GetSettingKey(string key)
        {
            return appSettings[GetEnvironmentKey()] + "." + key;
        }

        public static string GetEnvironmentSpecificSetting(string key, bool encrypted = false)
        {
            var environmentSpecificKey = GetSettingKey(key);
            if (appSettings[environmentSpecificKey] != null)
            {
                Console.WriteLine("Config is using Environemnt Specific Setting Key: {0} Value: {1}", environmentSpecificKey, appSettings[environmentSpecificKey]);

                if (encrypted)
                    return ConfigProtect.Decrypt(appSettings[environmentSpecificKey]);

                return appSettings[environmentSpecificKey];
            }
            return "";
        }

        public static string GetSetting(string key)
        {
            return appSettings[key];
        }

        // Connection Strings
        public static string GetConnectionString(string databaseName)
        {
            // First search for Hostname specific conn string eg: tcukcltstcpaw01.Blueline
            var hostSpecificKey = HostName + "." + databaseName;
            if (connStrings[hostSpecificKey] != null)
                return connStrings[hostSpecificKey].ToString();

            // Next search defualt eg: Blueline
            if (connStrings[databaseName] != null)
                return connStrings[databaseName].ToString();

            return "";
        }

    }
}
