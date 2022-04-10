using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rock.Framework.Extensions;
using rock.Core.Domains.Products;
namespace rock.Core.Data.Mappings
{
  public class ProductColorMap : IEntityTypeConfiguration<ProductColor>
  {
    public void Configure(EntityTypeBuilder<ProductColor> builder)
    {
      builder.HasKey(x => new { x.ProductId, x.ColorId });
      builder.Property(x => x.ProductId).IsRequired();
      builder.Property(x => x.ColorId).IsRequired();
      builder.HasRowVersion();
      builder.HasOne(x => x.Color)
             .WithMany()
             .HasForeignKey(x => x.ColorId);
      builder.HasOne(x => x.Product)
             .WithMany(x => x.ProductColors)
             .HasForeignKey(x => x.ProductId);
    }
  }
}