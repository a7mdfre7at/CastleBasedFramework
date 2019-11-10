using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Castle.Framework.DependencyInjection.Installers
{
    public class MvcInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var assemblies = container.Resolve<Assembly[]>();
            IEnumerable<Type> mvcInstallers = assemblies.SelectMany(assembly => assembly.GetExportedTypes().Where(t => t.IsClass
                                                                                                                    && !t.IsAbstract
                                                                                                                    && !t.IsGenericTypeDefinition
                                                                                                                    && typeof(IMvcInstaller).IsAssignableFrom(t)));

            foreach (var installer in mvcInstallers)
                container.Install(Activator.CreateInstance(installer).As<IWindsorInstaller>());
        }
    }
}