using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using rock.Framework.Extensions;
using rock.Core.Domains.Commons;

namespace rock.Core.Data.Mappings
{
  public class CityMap : IEntityTypeConfiguration<City>
  {
    public void Configure(EntityTypeBuilder<City> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasRowVersion();
      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.Name).IsRequired();

      builder.HasOne(x => x.Province)
             .WithMany(x => x.Cities)
             .HasForeignKey(x => x.ProvinceId);

    }
  }
}