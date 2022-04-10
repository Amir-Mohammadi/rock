using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rock.Core.Domains.Commons;
using rock.Framework.Extensions;

namespace rock.Core.Data.Mappings
{
  public class StaticMap : IEntityTypeConfiguration<Static>
  {
    public void Configure(EntityTypeBuilder<Static> builder)
    {
      builder.HasKey(x => x.Id);
      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.Key).IsRequired();
      builder.Property(x => x.Value);
      builder.Property(x => x.Value);
      builder.Property(x => x.StaticType).IsRequired();
      builder.HasRowVersion();
    }
  }
}