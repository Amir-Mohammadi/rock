using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rock.Framework.Extensions;
using rock.Core.Domains.Orders;
namespace rock.Core.Data.Mappings
{
  public class TransportMap : IEntityTypeConfiguration<Transport>
  {
    public void Configure(EntityTypeBuilder<Transport> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasRowVersion();
      builder.HasTimestamp();
      builder.HasDescription();
      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.OrderId).IsRequired();
      builder.Property(x => x.FromCityId).IsRequired();
      builder.Property(x => x.ToCityId).IsRequired();
      builder.Property(x => x.Cost).IsRequired();

      builder.HasOne(x => x.FromCity)
             .WithMany()
             .HasForeignKey(x => x.FromCityId);

      builder.HasOne(x => x.ToCity)
             .WithMany()
             .HasForeignKey(x => x.ToCityId);
    }
  }
}