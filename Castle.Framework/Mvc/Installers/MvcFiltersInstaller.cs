using System.Web.Mvc;
using Castle.Framework.DependencyInjection.Installers;
using Castle.Framework.Filters;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Castle.Framework.Mvc
{
    public class MvcFiltersInstaller : IMvcInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            FilterProviders.Providers.Add(new MvcFilterProvider(container));
        }
    }
}