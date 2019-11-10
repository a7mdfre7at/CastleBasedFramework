using System.Reflection;
using Castle.DependencyInjection.Extensions;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Castle.Framework.DependencyInjection.Installers
{
    public class DependenciesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var assemblies = container.Resolve<Assembly[]>();
            container.RegisterDependencies(assemblies);
        }
    }
}
