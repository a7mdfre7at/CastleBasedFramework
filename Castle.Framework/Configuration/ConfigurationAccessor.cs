using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;

namespace Castle.Framework.Configuration
{
    public class ConfigurationAccessor : IConfigurationAccessor
    {
        public virtual NameValueCollection Configurations => ConfigurationManager.AppSettings;

        public virtual string Get(string key)
        {
            return Configurations[key];
        }

        public virtual T Get<T>(string key)
        {
            return Get<T>(key, default(T));
        }

        public virtual T Get<T>(string key, T defaultValue)
        {
            try
            {
                if (!Configurations.AllKeys.Contains(key))
                    return defaultValue;

                return (T)Convert.ChangeType(Configurations[key], typeof(T));
            }
            catch (Exception ex)
            {
                return defaultValue;
            }
        }
    }
}
