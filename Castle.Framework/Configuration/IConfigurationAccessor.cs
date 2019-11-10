using System.Collections.Specialized;
using Castle.Framework.DependencyInjection;

namespace Castle.Framework.Configuration
{
    public interface IConfigurationAccessor : ITransientDependency
    {
        NameValueCollection Configurations { get; }
        string Get(string key);
        T Get<T>(string key);
        T Get<T>(string key, T defaultValue);
    }
}
