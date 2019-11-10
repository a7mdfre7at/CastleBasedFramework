using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Castle.Framework
{
    public static class AssemblyAccessor
    {
        public static Assembly[] Assemblies { get; private set; }

        static AssemblyAccessor()
        {
            Assemblies = GetLocalReferencedAssemblies();
        }
        private static Assembly[] GetLocalReferencedAssemblies()
        {
            // <add key="Assembly_Search_Pattern" value="Koder.*.dll"/>

            var setting = ConfigurationManager.AppSettings["Assembly_Search_Pattern"] ?? string.Empty;

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => Regex.IsMatch(a.FullName, setting)).ToArray();
            return assemblies;
        }
    }
}
