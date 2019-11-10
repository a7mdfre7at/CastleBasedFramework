using System.Reflection;
using Castle.Windsor;

namespace Castle.Framework.DependencyInjection
{
    public abstract class DependencyRegistration
    {
        public DependencyRegistration() { }
        public abstract void RegisterDependencies(IWindsorContainer container, params Assembly[] assemblies);
    }
}
