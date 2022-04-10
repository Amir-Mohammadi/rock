using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using rock.Framework.Extensions;
using rock.Core.Domains.Warehousing;

namespace rock.Core.Data.Mappings
{
  public class ShippingItemMap : IEntityTypeConfiguration<ShippingItem>
  {
    public void Configure(EntityTypeBuilder<ShippingItem> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasRowVersion();
      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.ShippingId).IsRequired();
      builder.Property(x => x.OrderItemId).IsRequired();

      builder.HasOne(x => x.Shipping).WithMany(x => x.Items).HasForeignKey(x => x.ShippingId);
      builder.HasOne(x => x.OrderItem).WithOne().HasForeignKey<ShippingItem>(x => x.OrderItemId);

    }
  }
}