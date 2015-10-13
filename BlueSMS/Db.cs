using BlueSMS.Data;
using Simple.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueSMS
{
    public class Db
    {
        public static string DbConnectionString { get; set; }
        public static string Environment { get; set; }




        public static string GetSetting(string settingKey)
        {
            var _db = Database.OpenConnection(DbConnectionString);
            Setting setting = _db.Settings.Find(_db.Settings.SettingKey == settingKey);
            return setting.SettingValue;
        }

        public static List<Setting> GetAllSettings()
        {
            int environmentLength = Environment.Length;
            var _db = Database.OpenConnection(DbConnectionString);
            List<Setting> settings = _db.Settings.All();
            var settingsFiltered = new List<Setting>();
            foreach (var setting in settings)
            {
                if (setting.SettingKey.Substring(0, environmentLength) == Environment)
                {
                    settingsFiltered.Add(setting);
                }
            }
            return settingsFiltered;
        }

        public static List<Reminder> GetPaymentReminders()
        {
            var _db = Database.OpenConnection(DbConnectionString);
            return _db.PaymentReminders.All();
        }

        public static List<Reminder> GetPaymentRemindersReport()
        {
            var _db = Database.OpenConnection(DbConnectionString);
            return _db.PaymentRemindersReport.All();
        }


    }
}
