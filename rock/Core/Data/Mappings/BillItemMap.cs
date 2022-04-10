using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using rock.Framework.Extensions;
using rock.Core.Domains.Financial;

namespace rock.Core.Data.Mappings
{
  public class BillItemMap : IEntityTypeConfiguration<BillItem>
  {
    public void Configure(EntityTypeBuilder<BillItem> builder)
    {
      builder.HasKey(x => x.Id);
      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.OrderItemId).IsRequired();

      builder.HasOne(x => x.OrderItem)
             .WithOne()
             .HasForeignKey<BillItem>(x => x.OrderItemId);

      builder.HasOne(x => x.Bill)
             .WithMany(x => x.Items)
             .HasForeignKey(x => x.BillId);
    }
  }
}