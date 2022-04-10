using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using rock.Framework.Extensions;
using rock.Core.Domains.Warehousing;

namespace rock.Core.Data.Mappings
{
  public class ShippingMap : IEntityTypeConfiguration<Shipping>
  {
    public void Configure(EntityTypeBuilder<Shipping> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasDescription();
      builder.HasRowVersion();
      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.DocumentId).IsRequired();

      builder.HasOne(x => x.Document).WithMany().HasForeignKey(x => x.DocumentId);
      builder.HasMany(x => x.Items).WithOne(x => x.Shipping).HasForeignKey(x => x.ShippingId);
    }
  }
}