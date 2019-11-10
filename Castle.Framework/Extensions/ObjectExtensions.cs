using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;

public static class ObjectExtensions
{
    public static bool Is<T>(this object obj) where T : class
    {
        return Is(obj, typeof(T));
    }

    public static bool Is(this object obj, Type type)
    {
        return obj != null ? obj.GetType() == type : false;
    }

    public static T As<T>(this object obj) where T : class
    {
        return obj as T;
    }

    public static IDictionary<string, object> AsDictionary(this object source, BindingFlags bindingAttr = BindingFlags.Public | BindingFlags.Instance)
    {
        return source.GetType().GetProperties(bindingAttr).ToDictionary
        (
            propInfo => propInfo.Name,
            propInfo => propInfo.GetValue(source, null)
        );
    }

    public static void CopyValues(this object target, object source,params string[]  IgnoreCopyProperties)
    {
        PropertyInfo[] targetProperties = target.GetType().GetProperties();
        Type targetType = target.GetType();

        PropertyInfo[] sourceProperties = source.GetType().GetProperties();

        foreach (PropertyInfo sourceProperty in sourceProperties)
        {
            if (!sourceProperty.CanRead || sourceProperty.GetIndexParameters().Length > 0)
            {
                continue;
            }

            PropertyInfo targetProperty = targetType.GetProperty(sourceProperty.Name);

            if (targetProperty != null && targetProperty.CanWrite)
            {
                if (IgnoreCopyProperties.Contains(targetProperty.Name))
                {
                    continue;
                }
                targetProperty.SetValue(target, sourceProperty.GetValue(source, null), null);
            }
        }
    }

    public static bool In<T>(this T obj, params T[] args)
    {
        return args.Contains(obj);
    }

    public static string ToQueryString(this object source)
    {
        return source.AsDictionary().ToQueryString();
    }

    /// <summary>
    /// Trim all String properties of the given object
    /// </summary>
    /// <param name="source"></param>
    public static void TrimStringProperties(this object source)
    {
        var stringProperties = source.GetType().GetProperties()
                          .Where(p => p.PropertyType == typeof(string));

        foreach (var stringProperty in stringProperties)
        {
            string currentValue = (string)stringProperty.GetValue(source, null);
            if (currentValue != null)
            {
                stringProperty.SetValue(source, currentValue.Trim(), null);
            }
        }
    }


    /// <summary>
    /// Set all null String properties of the given object to rmpty string
    /// </summary>
    /// <param name="source"></param>
    public static void SetNullStringsPropertiesToEmpty(this object source)
    {
        var stringProperties = source.GetType().GetProperties()
                          .Where(p => p.PropertyType == typeof(string));

        foreach (var stringProperty in stringProperties)
        {
            string currentValue = (string)stringProperty.GetValue(source, null);
            if (currentValue == null)
            {
                stringProperty.SetValue(source, string.Empty, null);
            }
        }
    }


    private static string SerializeQueryStringValue(string key, List<KeyValuePair<string, object>> values)
    {
        if (values.Count == 1)
            return $"{(!string.IsNullOrWhiteSpace(key) ? key + "=" : "")}{values.First().Value}";
        else
        {
            var results = new string[values.Count];
            for (var i = 0; i < values.Count; i++)
            {
                results[i] = $"{key}[{i}]={values[i].Value}";
            }
            return string.Join("&", results);
        }
    }

    private static IEnumerable<KeyValuePair<string, object>> ExtractQueryStringValues(object source)
    {
        var values = source
            .GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(o => o.GetValue(source, null) != null);

        foreach (var value in values)
        {
            if (!typeof(string).IsAssignableFrom(value.PropertyType)
                && typeof(IEnumerable).IsAssignableFrom(value.PropertyType))
            {
                var items = value.GetValue(source) as IList;
                if (items.Count > 0)
                {
                    for (int i = 0; i < items.Count; i++)
                    {
                        yield return new KeyValuePair<string, object>(value.Name, ToQueryString(items[i]));
                    }
                }
            }
            else if (value.PropertyType.IsComplex())
            {
                yield return new KeyValuePair<string, object>("", ToQueryString(value.GetValue(source)));
            }
            else
            {
                yield return new KeyValuePair<string, object>(value.Name, ConvertToUrlString(value.GetValue(source)));
            }
        }
    }

    private static string ConvertToUrlString(object value)
    {
        if (value == null)
            return string.Empty;

        if (value.GetType() == typeof(DateTime))
            return HttpUtility.UrlEncode(((DateTime)value).ToString(CultureInfo.InvariantCulture));

        return HttpUtility.UrlEncode(value.ToString());
    }

    private static bool IsComplex(this Type type)
    {
        var typeInfo = type.GetTypeInfo();
        if (typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            return IsComplex(typeInfo.GetGenericArguments()[0]);
        }
        return !(typeInfo.IsPrimitive
          || typeInfo.IsEnum
          || type.Equals(typeof(DateTime))
          || type.Equals(typeof(Guid))
          || type.Equals(typeof(string))
          || type.Equals(typeof(decimal)));
    }
}
