using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rock.Framework.Extensions;
using rock.Core.Domains.Products;
namespace rock.Core.Data.Mappings
{
  public class ProductImageMap : IEntityTypeConfiguration<ProductImage>
  {
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasRowVersion();
      builder.HasImage();
      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.Order).IsRequired();
      builder.Property(x => x.ProductId).IsRequired();
      builder.HasOne(x => x.Product)
             .WithMany(x => x.ProductImages)
             .HasForeignKey(x => x.ProductId);
    }
  }
}