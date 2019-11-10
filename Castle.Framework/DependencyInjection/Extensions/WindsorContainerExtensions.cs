using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Castle.Framework.DependencyInjection;
using Castle.Framework.DependencyInjection.Installers;
using Castle.MicroKernel;
using Castle.MicroKernel.ComponentActivator;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace Castle.DependencyInjection.Extensions
{
    public static class WindsorContainerExtensions
    {
        public static void RegisterDependencies(this IWindsorContainer container, params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
                container.RegisterDependency(assembly);

            foreach (var registration in assemblies.SelectMany(assembly => assembly.GetExportedTypes<DependencyRegistration>()))
                Activator.CreateInstance(registration).As<DependencyRegistration>().RegisterDependencies(container, assemblies);
        }

        public static void RegisterDependency(this IWindsorContainer container, Assembly assembly)
        {
            BasedOnDescriptor singletonDescriptor = Classes.FromAssembly(assembly).BasedOn<ISingletonDependency>().WithServiceAllInterfaces().LifestyleSingleton();
            container.Register(singletonDescriptor);

            BasedOnDescriptor transientDescriptor = Classes.FromAssembly(assembly).BasedOn<ITransientDependency>().WithServiceAllInterfaces().LifestyleTransient();
            container.Register(transientDescriptor);
        }

        public static void RunInstallers(this IWindsorContainer container, params Assembly[] assemblies)
        {
            foreach (var installer in assemblies.SelectMany(assembly => assembly.GetExportedTypes().Where(t => !t.IsAbstract 
                                                                                                            && typeof(IWindsorInstaller).IsAssignableFrom(t) 
                                                                                                            && !typeof(IMvcInstaller).IsAssignableFrom(t) 
                                                                                                            && !typeof(IWebApiInstaller).IsAssignableFrom(t)
                                                                                                            && t.Namespace != "Castle.Windsor.Installer"
                                                                                                          ).ToArray()))
                container.Install(FormatterServices.GetUninitializedObject(installer).As<IWindsorInstaller>());
        }

        public static void InjectProperties(this IKernel kernel, object target)
        {
            var type = target.GetType();
            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (property.CanWrite && kernel.HasComponent(property.PropertyType))
                {
                    var value = kernel.Resolve(property.PropertyType);
                    try
                    {
                        property.SetValue(target, value, null);
                    }
                    catch (Exception ex)
                    {
                        var message = string.Format("Error setting property {0} on type {1}, See inner exception for more information.", property.Name, type.FullName);
                        throw new ComponentActivatorException(message, ex, null);
                    }
                }
            }
        }
    }
}