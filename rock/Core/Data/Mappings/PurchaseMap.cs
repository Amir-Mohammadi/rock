using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using rock.Framework.Extensions;
using rock.Core.Domains.Financial;

namespace rock.Core.Data.Mappings
{
  public class PurchaseMap : IEntityTypeConfiguration<Purchase>
  {
    public void Configure(EntityTypeBuilder<Purchase> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasRowVersion();
      builder.HasDescription();
      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.DocumentId).IsRequired();
      builder.Property(x => x.PaymentSapNo);
      builder.Property(x => x.PaymentTransactionNo);
      builder.Property(x => x.PaymentPayload);
      builder.Property(x => x.RRN);
      builder.Property(x => x.VerifyPayload);
      builder.Property(x => x.IsVerfied);
      builder.Property(x => x.OrderId).IsRequired();
      builder.HasOne(x => x.Document).WithMany().HasForeignKey(x => x.DocumentId);
      builder.HasMany(x => x.Items).WithOne(x => x.Purchase).HasForeignKey(x => x.PurchaseId);
    }
  }
}