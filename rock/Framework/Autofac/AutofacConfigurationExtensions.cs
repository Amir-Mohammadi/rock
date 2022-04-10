using System.Reflection;
using Autofac;
using rock.Core.Data;

namespace rock.Framework.Autofac
{
  public static class AutofacConfigurationExtensions
  {
    public static void AddAutofacDependencyServices(this ContainerBuilder containerBuilder)
    {

      var currentAssembly = Assembly.GetExecutingAssembly();

      containerBuilder.RegisterAssemblyTypes(currentAssembly)
      .AssignableTo<IScopedDependency>()
      .AsImplementedInterfaces()
      .InstancePerLifetimeScope();

      containerBuilder.RegisterAssemblyTypes(currentAssembly)
        .AssignableTo<ITransientDependency>()
        .AsImplementedInterfaces()
        .InstancePerDependency();

      containerBuilder.RegisterAssemblyTypes(currentAssembly)
        .AssignableTo<ISingletonDependency>()
        .AsImplementedInterfaces()
        .SingleInstance();
      
      containerBuilder.RegisterGeneric(typeof(Repository<>))
        .As(typeof(IRepository<>))
        .InstancePerLifetimeScope(); 

    }
  }
}