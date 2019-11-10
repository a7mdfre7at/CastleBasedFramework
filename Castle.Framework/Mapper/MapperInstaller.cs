using System.Linq;
using AutoMapper;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.Framework.DependencyInjection;
using System.Reflection;

namespace Castle.Framework.Mapper
{
    public class MapperInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var assemblies = container.Resolve<Assembly[]>();

            container.Register(Component.For<MapperConfiguration>().UsingFactoryMethod(k =>
            {
                var config = new MapperConfiguration(cfg =>
                {
                    foreach (var profile in assemblies.SelectMany(s => s.GetExportedTypes<Profile>()))
                        cfg.AddProfile(profile);
                });

                return config;
            }).LifestyleSingleton());

            container.Register(Component.For<IMapper>().UsingFactoryMethod(k =>
            {
                return k.Resolve<MapperConfiguration>().CreateMapper();

            }).LifestyleTransient());
        }
    }
}
