using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using rock.Framework.Extensions;
using rock.Core.Domains.Commons;

namespace rock.Core.Data.Mappings
{
  public class ProvinceMap : IEntityTypeConfiguration<Province>
  {
    public void Configure(EntityTypeBuilder<Province> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasRowVersion();
      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.Name).IsRequired();
      builder.Property(x => x.AreaCode).IsRequired();

      builder.HasMany(x => x.Cities).WithOne(x => x.Province).HasForeignKey(x => x.ProvinceId);
    }
  }
}