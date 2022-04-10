
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rock.Core.Domains.Shops;
using rock.Framework.Extensions;
namespace rock.Core.Data.Mappings
{
  public class ShopStuffPriceMap : IEntityTypeConfiguration<ShopStuffPrice>
  {
    public void Configure(EntityTypeBuilder<ShopStuffPrice> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasRowVersion();
      builder.HasRemoveable();
      builder.Property(x => x.Price);
      builder.Property(x => x.StuffId);
      builder.Property(x => x.CreatedAt);
      builder.HasOne(x => x.Stuff)
             .WithMany(x => x.ShopStuffPrices)
             .HasForeignKey(x => x.StuffId);
      builder.HasOne(x => x.Shop)
             .WithMany()
             .HasForeignKey(x => x.ShopId);
      builder.HasOne(x => x.ShopStuff)
             .WithMany(x => x.ShopStuffPrices)
             .HasForeignKey(x => new { x.ShopId, x.StuffId });
      builder.HasOne(x => x.Color)
             .WithMany()
             .HasForeignKey(x => x.ColorId);
      builder.HasOne(x => x.StuffColor)
             .WithMany()
             .HasForeignKey(x => new { x.StuffId, x.ColorId });
    }
  }
}