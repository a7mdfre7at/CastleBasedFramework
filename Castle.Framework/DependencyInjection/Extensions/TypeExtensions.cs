using System;
using System.Collections.Generic;

namespace Castle.DependencyInjection.Extensions
{
    public static class TypeExtensions
    {
        public static Type[] GetInterfacesAndAbstractClasses(this Type type)
        {
            if (type.BaseType == null)
                return new Type[0];

            var baseTypes = new List<Type>(type.GetInterfaces());
            var currentType = type;

            while ((currentType = currentType.BaseType) != null)
            {
                if (currentType.IsInterface || currentType.IsAbstract)
                    baseTypes.Add(currentType);
            }

            return baseTypes.ToArray();
        }
    }
}
