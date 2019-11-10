using System.Reflection;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Castle.Framework.DependencyInjection;
using Castle.Services;

namespace Castle.Mvc
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Injector.Setup(GetAssemblies());
        }

        //private Assembly[] GetAssemblies()
        //{
        //    return new Assembly[] {
        //        Assembly.GetAssembly(typeof(IocContainer)),
        //        Assembly.GetAssembly(typeof(MvcApplication)),
        //        Assembly.GetAssembly(typeof(IUserService))
        //    };
        //}
    }
}
