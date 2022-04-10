
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rock.Core.Domains.Shops;
using rock.Framework.Extensions;
namespace rock.Core.Data.Mappings
{
  public class ShopProfileMap : IEntityTypeConfiguration<ShopProfile>
  {
    public void Configure(EntityTypeBuilder<ShopProfile> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasRowVersion();
      builder.HasTimestamp();

      builder.Property(x => x.ShopId).IsRequired();
      builder.Property(x => x.Address);
      builder.Property(x => x.PostalCode);
      builder.Property(x => x.Telephone);
      builder.Property(x => x.Website);
      builder.HasOne(x => x.Shop)
             .WithOne(x => x.Profile)
             .HasForeignKey<ShopProfile>(x => x.ShopId);
    }
  }
}