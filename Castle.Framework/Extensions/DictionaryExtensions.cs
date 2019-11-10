using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Castle.Extensions
{
    public static class DictionaryExtensions
    {
        public static T ToObject<T>(this IDictionary<string, object> source) where T : class, new()
        {
            var someObject = new T();
            var someObjectType = someObject.GetType();

            foreach (var item in source)
            {
                someObjectType
                         .GetProperty(item.Key)
                         .SetValue(someObject, item.Value, null);
            }

            return someObject;
        }

        public static string ToQueryString(this IDictionary<string, object> source)
        {
            return string.Join("&", source.Select(x => $"{x.Key}={ConvertToString(x.Value)}"));
        }

        private static string ConvertToString(object value)
        {
            if (value == null)
                return null;

            if (value.GetType() == typeof(DateTime))
                return ((DateTime)value).ToString(CultureInfo.InvariantCulture);

            return value.ToString();
        }
    }
}
