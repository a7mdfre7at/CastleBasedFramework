using Castle.MicroKernel.Registration;

namespace Castle.Framework.DependencyInjection.Installers
{
    public interface IMvcInstaller : IWindsorInstaller
    {
    }

    public interface IWebApiInstaller : IWindsorInstaller
    {
    }
}
