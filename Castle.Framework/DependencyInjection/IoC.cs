using System;
using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace Castle.Framework.DependencyInjection
{
    public static class IoC
    {
        private static IWindsorContainer container;

        public static void Initialize(IWindsorContainer windsorContainer)
        {
            container = windsorContainer;
        }

        public static void Register(params IRegistration[] registration)
        {
            container.Register(registration);
        }

        public static object Resolve(Type service)
        {
            return container.Resolve(service);
        }

        public static T[] ResolveAll<T>()
        {
            return Container.ResolveAll<T>();
        }

        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        public static T Resolve<T>(string name)
        {
            return Container.Resolve<T>(name);
        }

        public static T Resolve<T>(IEnumerable<KeyValuePair<string, object>> argument)
        {
            return container.Resolve<T>(argument);
        }

        public static IWindsorContainer Container
        {
            get
            {
                if (container == null)
                    throw new InvalidOperationException("The container has not been initialized!");

                return container;
            }
        }

        public static bool IsInitialized
        {
            get { return container != null; }
        }
    }
}