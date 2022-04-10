using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using rock.Core.Domains.Threads;
using rock.Framework.Extensions;

namespace rock.Core.Data.Mappings
{
  public class ThreadMap : IEntityTypeConfiguration<Thread>
  {
    public void Configure(EntityTypeBuilder<Thread> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasRowVersion();
      builder.HasRemoveable();
      builder.Property(x => x.Id).IsRequired();
    }
  }
}