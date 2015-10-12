using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueSMS
{
    public static class Config
    {

        public static string Environment { get { return CommonConfig.GetEnvironment(); } }

        // App Settings - Environment Specific - Encrypted
        public static string DatabaseConnString { get { return CommonConfig.GetEnvironmentSpecificSetting("DatabaseConnString", true); } }


        // Settings stored in DB
        public static string SmtpUsername { get { return Db.GetSetting(CommonConfig.GetSettingKey("SmtpUsername")); } }
        public static string SmtpPassword { get { return Db.GetSetting(CommonConfig.GetSettingKey("SmtpPassword")); } }

        public static string TwilioAccountSid { get { return Db.GetSetting(CommonConfig.GetSettingKey("TwilioAccountSid")); } }
        public static string TwilioAuthToken { get { return Db.GetSetting(CommonConfig.GetSettingKey("TwilioAuthToken")); } }
        public static string TwilioFromNumber { get { return Db.GetSetting(CommonConfig.GetSettingKey("TwilioFromNumber")); } }


    }
}
