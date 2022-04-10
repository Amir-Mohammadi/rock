using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rock.Framework.Extensions;
using rock.Core.Domains.Financial;
namespace rock.Core.Data.Mappings
{
  public class BillMap : IEntityTypeConfiguration<Bill>
  {
    public void Configure(EntityTypeBuilder<Bill> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasDescription();
      builder.HasRowVersion();
      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.DocumentId).IsRequired();
      builder.HasOne(x => x.Document)
             .WithOne()
             .HasForeignKey<Bill>(x => x.DocumentId);
      builder.HasMany(x => x.Items)
             .WithOne(x => x.Bill)
             .HasForeignKey(x => x.BillId);
    }
  }
}