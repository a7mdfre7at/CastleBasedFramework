using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Castle.Framework.DependencyInjection.Installers
{
    public class WebApiInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var assemblies = container.Resolve<Assembly[]>();
            IEnumerable<Type> webApiInstallers = assemblies.SelectMany(assembly => assembly.GetExportedTypes().Where(t => t.IsClass
                                                                                                                       && !t.IsAbstract
                                                                                                                       && !t.IsGenericTypeDefinition
                                                                                                                       && typeof(IWebApiInstaller).IsAssignableFrom(t)));

            foreach (var installer in webApiInstallers)
                container.Install(Activator.CreateInstance(installer).As<IWindsorInstaller>());
        }
    }
}