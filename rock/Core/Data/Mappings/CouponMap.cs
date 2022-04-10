using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rock.Framework.Extensions;
using rock.Core.Domains.Orders;
namespace rock.Core.Data.Mappings
{
  public class CouponMap : IEntityTypeConfiguration<Coupon>
  {
    public void Configure(EntityTypeBuilder<Coupon> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasTimestamp();
      builder.HasRowVersion();
      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.CouponCode).IsRequired();
      builder.Property(x => x.ExpiryDate);
      builder.Property(x => x.MaxQuantities);
      builder.Property(x => x.Value);
      builder.Property(x => x.Active);
      builder.Property(x => x.MaxQuantitiesPerUser);
    }
  }
}