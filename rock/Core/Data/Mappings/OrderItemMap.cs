using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using rock.Framework.Extensions;
using rock.Core.Domains.Orders;

namespace rock.Core.Data.Mappings
{
  public class OrderItemMap : IEntityTypeConfiguration<OrderItem>
  {
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasRowVersion();
      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.OrderId).IsRequired();
      builder.Property(x => x.CartItemId).IsRequired();
      builder.Property(x => x.ShopId);

      builder.HasOne(x => x.Order)
             .WithMany(x => x.Items)
             .HasForeignKey(x => x.OrderId);

      builder.HasOne(x => x.CartItem)
             .WithOne()
             .HasForeignKey<OrderItem>(x => x.CartItemId);

      builder.HasMany(x => x.Statuses)
             .WithOne(x => x.OrderItem)
             .HasForeignKey(x => x.OrderItemId);

      builder.HasOne(x => x.Shop)
             .WithMany(x => x.OrderItems)
             .HasForeignKey(x => x.ShopId);

      builder.HasOne(x => x.Transport)
             .WithMany(x => x.OrderItems)
             .HasForeignKey(x => x.TransportId);

      builder.HasOne(x => x.LatestStatus)
             .WithOne()
             .HasForeignKey<OrderItem>(x => x.LatestOrderItemStatusId);

    }
  }
}