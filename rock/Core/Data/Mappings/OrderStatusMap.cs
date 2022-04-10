using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rock.Core.Domains.Orders;
using rock.Framework.Extensions;

namespace rock.Core.Data.Mappings
{
  public class OrderStatusMap : IEntityTypeConfiguration<OrderStatus>
  {
    public void Configure(EntityTypeBuilder<OrderStatus> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasRowVersion();
      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.OrderId).IsRequired();
      builder.Property(x => x.Description).HasMaxLength(300);
      builder.Property(x => x.OrderStatusType).IsRequired();
      builder.Property(x => x.UserId).IsRequired();
      builder.Property(x => x.CreatedAt).IsRequired();

      builder.HasOne(x => x.User)
             .WithMany()
             .HasForeignKey(x => x.UserId);

      builder.HasOne(x => x.Order)
             .WithMany(x => x.OrderStatuses)
             .HasForeignKey(x => x.OrderId);
    }
  }
}