using System;
using System.Collections.Generic;
using System.Linq;

namespace Castle.Extensions
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

        public static bool IsAssignableToGenericType(this Type givenType, Type genericType)
        {
            if (givenType == null || genericType == null)
            {
                return false;
            }

            return givenType == genericType
              || givenType.MapsToGenericTypeDefinition(genericType)
              || givenType.HasInterfaceThatMapsToGenericTypeDefinition(genericType)
              || givenType.BaseType.IsAssignableToGenericType(genericType);
        }

        private static bool HasInterfaceThatMapsToGenericTypeDefinition(this Type givenType, Type genericType)
        {
            return givenType
              .GetInterfaces()
              .Where(x => x.IsGenericType)
              .Any(x => x.GetGenericTypeDefinition() == genericType);
        }

        private static bool MapsToGenericTypeDefinition(this Type givenType, Type genericType)
        {
            return genericType.IsGenericTypeDefinition
              && givenType.IsGenericType
              && givenType.GetGenericTypeDefinition() == genericType;
        }
    }
}
