
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rock.Core.Domains.Shops;
using rock.Framework.Extensions;
namespace rock.Core.Data.Mappings
{
  public class ShopMap : IEntityTypeConfiguration<Shop>
  {
    public void Configure(EntityTypeBuilder<Shop> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasRowVersion();
      builder.Property(x => x.Active).IsRequired();
      builder.Property(x => x.CreatedAt).IsRequired();
      builder.Property(x => x.CityId).IsRequired();
      builder.Property(x => x.OwnerId).IsRequired();
      builder.Property(x => x.Name).IsRequired();
      builder.Property(x => x.WarehouseId).IsRequired();
      builder.HasOne(x => x.Owner)
             .WithOne()
             .HasForeignKey<Shop>(x => x.OwnerId);
      builder.HasOne(x => x.Warehouse)
             .WithOne()
             .HasForeignKey<Shop>(x => x.WarehouseId);
      builder.HasOne(x => x.City)
             .WithMany()
             .HasForeignKey(x => x.CityId);

    }
  }
}