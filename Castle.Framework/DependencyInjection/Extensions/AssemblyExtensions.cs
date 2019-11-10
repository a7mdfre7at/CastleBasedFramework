using System;
using System.Linq;
using System.Reflection;

namespace Castle.Framework.DependencyInjection
{
    public static class AssemblyExtensions
    {
        public static Type[] GetExportedTypes<T>(this Assembly assembly)
        {
            return assembly.GetExportedTypes()
                .Where(t => !t.IsAbstract && typeof(T).IsAssignableFrom(t)).ToArray();
        }
    }
}
