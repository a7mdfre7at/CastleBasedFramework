using System;
using System.Linq;
using System.Reflection;
using Castle.Framework.Configuration;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Refit;

namespace Castle.Framework.Communication
{
    public class RefitInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            foreach (var serviceType in AssemblyAccessor.Assemblies.SelectMany(assembly => assembly.GetExportedTypes().Where(t => typeof(IApi).IsAssignableFrom(t))))
                Register(container, serviceType);
        }

        private void Register(IWindsorContainer container, Type serviceType)
        {
            if (!serviceType.IsInterface || !typeof(IApi).IsAssignableFrom(serviceType) || serviceType == typeof(IApi))
                return;

            var prefix = serviceType.GetCustomAttribute<PrefixAttribute>()?.Path ?? "";

            container.Register(Component.For(serviceType).UsingFactoryMethod(k =>
            {
                return RestService.For(serviceType, string.Concat(k.Resolve<IConfigurationAccessor>().Get<string>("Api_Base_Address"), prefix));
            }));
        }
    }
}