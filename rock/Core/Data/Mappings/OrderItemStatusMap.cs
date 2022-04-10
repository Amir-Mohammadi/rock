using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using rock.Framework.Extensions;
using rock.Core.Domains.Orders;

namespace rock.Core.Data.Mappings
{
  public class OrderItemStatusMap : IEntityTypeConfiguration<OrderItemStatus>
  {
    public void Configure(EntityTypeBuilder<OrderItemStatus> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasRowVersion();
      builder.HasDescription();
      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.Type).IsRequired();
      builder.Property(x => x.OrderItemId).IsRequired();
      builder.Property(x => x.CreatedAt).IsRequired();

      builder.HasOne(x => x.OrderItem)
             .WithMany(x => x.Statuses)
             .HasForeignKey(x => x.OrderItemId);


    }
  }
}