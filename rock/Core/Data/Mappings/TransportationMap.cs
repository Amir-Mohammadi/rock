using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rock.Framework.Extensions;
using rock.Core.Domains.Commons;
namespace rock.Core.Data.Mappings
{
  public class TransportationMap : IEntityTypeConfiguration<Transportation>
  {
    public void Configure(EntityTypeBuilder<Transportation> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasRowVersion();
      builder.HasRemoveable();
      builder.HasDescription();
      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.FromCityId).IsRequired();
      builder.Property(x => x.ToCityId).IsRequired();
      builder.Property(x => x.Cost).IsRequired();
      builder.Property(x => x.Distance).IsRequired();
      builder.Property(x => x.CreatedAt).IsRequired();
      builder.HasOne(x => x.FromCity)
             .WithMany()
             .HasForeignKey(x => x.FromCityId);
      builder.HasOne(x => x.ToCity)
             .WithMany()
             .HasForeignKey(x => x.ToCityId);
    }
  }
}