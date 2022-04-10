using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rock.Framework.Extensions;
using rock.Core.Domains.Warehousing;
namespace rock.Core.Data.Mappings
{
  public class ShopInventoryModificationItemMap : IEntityTypeConfiguration<ShopInventoryModificationItem>
  {
    public void Configure(EntityTypeBuilder<ShopInventoryModificationItem> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasRowVersion();
      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.StuffId).IsRequired();
      builder.Property(x => x.ShopId).IsRequired();
      builder.Property(x => x.ColorId).IsRequired();
      builder.Property(x => x.ShopInventoryModificationId).IsRequired();
      builder.HasOne(x => x.ShopInventoryModification)
             .WithMany(x => x.Items)
             .HasForeignKey(x => x.ShopInventoryModificationId);
      builder.HasOne(x => x.Shop)
             .WithMany()
             .HasForeignKey(x => x.ShopId);
      builder.HasOne(x => x.Stuff)
             .WithMany()
             .HasForeignKey(x => x.StuffId);
      builder.HasOne(x => x.Color)
             .WithMany()
             .HasForeignKey(x => x.ColorId);
      builder.HasOne(x => x.ShopStuff)
             .WithMany()
             .HasForeignKey(x => new { x.ShopId, x.StuffId });
      builder.HasOne(x => x.StuffColor)
             .WithMany()
             .HasForeignKey(x => new { x.StuffId, x.ColorId });
    }
  }
}