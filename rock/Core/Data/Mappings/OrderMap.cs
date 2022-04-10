using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rock.Framework.Extensions;
using rock.Core.Domains.Orders;
using rock.Core.Domains.Payment;
namespace rock.Core.Data.Mappings
{
  public class OrderMap : IEntityTypeConfiguration<Order>
  {
    public void Configure(EntityTypeBuilder<Order> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasRowVersion();
      builder.HasTimestamp();
      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.CartId).IsRequired();
      builder.Property(x => x.CouponId);
      builder.Property(x => x.Tax).IsRequired();
      builder.Property(x => x.TotalAmount).IsRequired();
      builder.Property(x => x.Expiration);
      builder.Property(x => x.DocumentId);

      builder.HasOne(x => x.Cart)
             .WithOne(x => x.Order)
             .HasForeignKey<Order>(x => x.CartId);

      builder.HasMany(x => x.Items)
             .WithOne(x => x.Order)
             .HasForeignKey(x => x.OrderId);

      builder.HasOne(x => x.Document)
             .WithMany(x => x.Orders)
             .HasForeignKey(x => x.DocumentId);

      builder.HasOne(x => x.Coupon)
             .WithMany(x => x.Orders)
             .HasForeignKey(x => x.CouponId);

      builder.HasOne(x => x.LatestOrderStatus)
             .WithOne()
             .HasForeignKey<Order>(x => x.LatestOrderStatusId);

    }
  }
}