using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rock.Core.Domains.Products;
using rock.Framework.Extensions;

namespace rock.Core.Data.Mappings
{
  public class ProductShippingInfoMap : IEntityTypeConfiguration<ProductShippingInfo>
  {
    public void Configure(EntityTypeBuilder<ProductShippingInfo> builder)
    {
      builder.HasKey(x => x.ProductId);
      builder.Property(x => x.Height);
      builder.Property(x => x.Width);
      builder.Property(x => x.Length);
      builder.Property(x => x.Weight);
      builder.HasRowVersion();
      builder.HasTimestamp();

      builder.HasOne(x => x.Product)
             .WithOne(x => x.ProductShippingInfo)
             .HasForeignKey<ProductShippingInfo>(x => x.ProductId);
    }
  }
}