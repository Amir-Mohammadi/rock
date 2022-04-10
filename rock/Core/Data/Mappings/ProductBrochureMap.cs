using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using rock.Framework.Extensions;
using rock.Core.Domains.Products;

namespace rock.Core.Data.Mappings
{
  public class ProductBrochureMap : IEntityTypeConfiguration<ProductBrochure>
  {
    public void Configure(EntityTypeBuilder<ProductBrochure> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasRowVersion();
      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.ProductId).IsRequired();
      builder.Property(x => x.HTML).IsRequired();

      builder.HasMany(x => x.Attachments)
             .WithOne(x => x.Brochure)
             .HasForeignKey(x => x.BrochurId);

      builder.HasOne(x => x.Product)
             .WithOne(x => x.ProductBrochure)
             .HasForeignKey<ProductBrochure>(x => x.ProductId);

    }
  }
}