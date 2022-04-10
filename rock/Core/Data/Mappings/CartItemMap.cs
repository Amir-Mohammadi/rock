using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rock.Framework.Extensions;
using rock.Core.Domains.Orders;
namespace rock.Core.Data.Mappings
{
  public class CartItemMap : IEntityTypeConfiguration<CartItem>
  {
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasRowVersion();
      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.ProductId).IsRequired();
      builder.Property(x => x.ShopStuffPriceId);
      builder.Property(x => x.ProductPriceId).IsRequired();
      builder.Property(x => x.CartId).IsRequired();
      builder.Property(x => x.ColorId).IsRequired();
      builder.HasOne(x => x.Product)
             .WithMany()
             .HasForeignKey(x => x.ProductId);
      builder.HasOne(x => x.StuffPrice)
             .WithMany()
             .HasForeignKey(x => x.ShopStuffPriceId);
      builder.HasOne(x => x.ProductPrice)
             .WithMany()
             .HasForeignKey(x => x.ProductPriceId);
      builder.HasOne(x => x.Cart)
             .WithMany(x => x.Items)
             .HasForeignKey(x => x.CartId);
      builder.HasOne(x => x.Color)
             .WithMany()
             .HasForeignKey(x => x.ColorId);
      builder.HasOne(x => x.ProductColor)
             .WithMany()
             .HasForeignKey(x => new { x.ProductId, x.ColorId });
    }
  }
}