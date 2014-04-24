using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web;

namespace TFax.Web.CORE.COMMON.Helpers
{
    public class AppSettingsHelper
    {
        public static T GetValue<T>(string key, T defaultValue)
        {
            var value = ConfigurationManager.AppSettings[key];

            if (string.IsNullOrEmpty(value))
                return defaultValue;

            return (T)Convert.ChangeType(value, typeof(T));
        }

        public static T GetValue<T>(string key)
        {
            return GetValue(key, default(T));
        }

    }
}