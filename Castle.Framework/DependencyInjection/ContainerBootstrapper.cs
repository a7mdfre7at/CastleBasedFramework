using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Castle.DependencyInjection.Extensions;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace Castle.Framework.DependencyInjection
{
    public class ContainerBootstrapper : IContainerAccessor, IDisposable
    {
        ContainerBootstrapper(IWindsorContainer container)
        {
            Container = container;
        }

        public IWindsorContainer Container { get; }

        public static ContainerBootstrapper Bootstrap()
        {
            var container = new WindsorContainer();
            Assembly[] startupAssemblies = GetStartupAssemblies();
            container.Register(Component.For<IWindsorContainer>().Instance(container));
            container.Register(Component.For<Assembly[]>().Named("_startupAssemblies").Instance(startupAssemblies));
            container.RunInstallers(startupAssemblies);
            //container.Install(FromAssembly.This());
            IoC.Initialize(container);
            return new ContainerBootstrapper(container);
        }

        private static Assembly[] GetStartupAssemblies()
        {
            var setting = ConfigurationManager.AppSettings["Assembly_Search_Pattern"] ?? string.Empty;

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => Regex.IsMatch(a.FullName, setting)).ToArray();
            return assemblies;
        }

        public void Dispose()
        {
            Container.Dispose();
        }
    }
}