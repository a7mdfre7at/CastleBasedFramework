using System;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;

namespace Castle.Extensions
{
    public static class InvocationExtentions
    {
        public static void Invoke(this IInvocation invocation)
        {
            Assembly assembly = invocation.Method.DeclaringType?.Assembly;

            Type proxyClassType = assembly.GetTypes().Where(t => !t.IsAbstract && !t.IsInterface && invocation.Method.DeclaringType.IsAssignableFrom(t)).AsParallel().FirstOrDefault();
            object proxyClassObject = proxyClassType.GetConstructor(Type.EmptyTypes).Invoke(new object[0]);

            MethodInfo methodToInvoke = proxyClassType.GetMethod(invocation.Method.Name);
            invocation.ReturnValue = methodToInvoke.Invoke(proxyClassObject, invocation.Arguments);
        }
    }
}