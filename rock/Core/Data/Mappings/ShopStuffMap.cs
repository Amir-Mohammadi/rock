
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rock.Core.Domains.Shops;
using rock.Framework.Extensions;
namespace rock.Core.Data.Mappings
{
  public class ShopStuffMap : IEntityTypeConfiguration<ShopStuff>
  {
    public void Configure(EntityTypeBuilder<ShopStuff> builder)
    {
      builder.HasKey(x => new { x.ShopId, x.StuffId });
      builder.HasRowVersion();
      builder.HasRemoveable();
      builder.Property(x => x.ShopId);
      builder.Property(x => x.StuffId);
      builder.HasOne(x => x.Shop)
             .WithMany(x => x.ShopStuffs)
             .HasForeignKey(x => x.ShopId);
      builder.HasOne(x => x.Stuff)
             .WithMany(x => x.ShopStuffs)
             .HasForeignKey(x => x.StuffId);
    }
  }
}