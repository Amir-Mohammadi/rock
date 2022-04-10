using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using rock.Framework.Extensions;
using rock.Core.Domains.Products;

namespace rock.Core.Data.Mappings
{
  public class ProductBrochureAttachmentMap : IEntityTypeConfiguration<ProductBrochureAttachment>
  {
    public void Configure(EntityTypeBuilder<ProductBrochureAttachment> builder)
    {
      builder.HasKey(x => x.Id);
      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.FileId).IsRequired();
      builder.Property(x => x.BrochurId).IsRequired();

      builder.HasOne(x => x.File)
             .WithMany()
             .HasForeignKey(x => x.FileId);
      builder.HasOne(x => x.Brochure)
             .WithMany(x => x.Attachments)
             .HasForeignKey(x => x.BrochurId);

    }
  }
}