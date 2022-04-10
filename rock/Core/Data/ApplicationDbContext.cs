using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using rock.Framework.Extensions;
using rock.Framework.Autofac;

namespace rock.Core.Data
{
  public class ApplicationDbContext : DbContext, IScopedDependency
  {

    public ApplicationDbContext(DbContextOptions options)
         : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
      var assembly = Assembly.GetExecutingAssembly();
      var nameSpace = this.GetType().Namespace;
      modelBuilder.RegisterAllEntities(assembly: assembly, nameSpace: nameSpace);
      modelBuilder.RegisterIEntityTypeConfiguration(assembly: assembly, nameSpace: nameSpace);
      modelBuilder.AddRestrictDeleteBehaviorConvention();
      modelBuilder.AddPluralizingTableNameConvention();
    }
  }
}