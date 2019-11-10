using System.Reflection;
using System.Web.Mvc;
using Castle.Framework.DependencyInjection.Installers;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Castle.Framework.Mvc
{
    public class MvcControllersInstaller : IMvcInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IActionInvoker>().ImplementedBy<WindsorActionInvoker>().LifeStyle.Transient);

            var assemblies = container.Resolve<Assembly[]>();

            foreach (var assembly in assemblies)
            {
                BasedOnDescriptor descriptor = Classes.FromAssembly(assembly).BasedOn<IController>().If(c => c.Name.EndsWith("Controller")).LifestylePerWebRequest();
                container.Register(descriptor);
            }

            ControllerBuilder.Current.SetControllerFactory(new MvcControllerFactory(container));
        }
    }
}
