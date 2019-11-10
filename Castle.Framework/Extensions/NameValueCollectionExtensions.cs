using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace Castle.Extensions
{
    public static class NameValueCollectionExtensions
    {
        public static string ToQueryString(this NameValueCollection collection)
        {
            var array = (from key in collection.AllKeys
                         from value in collection.GetValues(key)
                         select $"{HttpUtility.UrlEncode(key)}={HttpUtility.UrlEncode(value)}")
                .ToArray();
            return string.Join("&", array);
        }

        public static string ToQueryString(this KeyValuePair<string, string>[] collection)
        {
            return string.Join("&", collection.Select(x => $"{HttpUtility.UrlEncode(x.Key)}={HttpUtility.UrlEncode(x.Value)}").ToArray());
        }
    }
}
