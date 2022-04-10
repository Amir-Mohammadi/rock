using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rock.Framework.Extensions;
using rock.Core.Domains.Orders;
namespace rock.Core.Data.Mappings
{
  public class CartMap : IEntityTypeConfiguration<Cart>
  {
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasTimestamp();
      builder.HasRowVersion();
      builder.HasRemoveable();
      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.UserId).IsRequired();
      builder.Property(x => x.CartStatus);
      builder.HasOne(x => x.User)
             .WithMany()
             .HasForeignKey(x => x.UserId);
      builder.HasOne(x => x.ProfileAddress)
             .WithMany(x => x.Carts)
             .HasForeignKey(x => x.ProfileAddressId);
    }
  }
}