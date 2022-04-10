using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rock.Core.Domains.Products;
using rock.Framework.Extensions;
namespace rock.Core.Data.Mappings
{
  public class ProductPriceMap : IEntityTypeConfiguration<ProductPrice>
  {
    public void Configure(EntityTypeBuilder<ProductPrice> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasRowVersion();
      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.CityId).IsRequired();
      builder.Property(x => x.ColorId).IsRequired();
      builder.Property(x => x.ProductId).IsRequired();
      builder.Property(x => x.Discount);
      builder.Property(x => x.MaxPrice).IsRequired();
      builder.Property(x => x.MinPrice).IsRequired();
      builder.Property(x => x.Price).IsRequired();
      builder.Property(x => x.IsPublished).IsRequired();
      builder.HasOne(x => x.Product)
             .WithMany(x => x.ProductPrices)
             .HasForeignKey(x => x.ProductId);
      builder.HasOne(x => x.Color)
             .WithMany()
             .HasForeignKey(x => x.ColorId);
      builder.HasOne(x => x.ProductColor)
             .WithMany()
             .HasForeignKey(x => new { x.ProductId, x.ColorId });
      builder.HasOne(x => x.City)
             .WithMany()
             .HasForeignKey(x => x.CityId);
    }
  }
}