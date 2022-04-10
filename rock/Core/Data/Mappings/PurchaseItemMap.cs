using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using rock.Framework.Extensions;
using rock.Core.Domains.Financial;

namespace rock.Core.Data.Mappings
{
  public class PurchaseItemMap : IEntityTypeConfiguration<PurchaseItem>
  {
    public void Configure(EntityTypeBuilder<PurchaseItem> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasRowVersion();
      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.PurchaseId).IsRequired();
      builder.Property(x => x.OrderItemId).IsRequired();

      builder.HasOne(x => x.Purchase).WithMany(x => x.Items).HasForeignKey(x => x.PurchaseId);
      builder.HasOne(x => x.OrderItem).WithOne().HasForeignKey<PurchaseItem>(x => x.OrderItemId);


    }
  }
}