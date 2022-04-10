using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rock.Core.Domains.Payment;
using rock.Framework.Extensions;

namespace rock.Core.Data.Mappings
{
  public class OrderPaymentMap : IEntityTypeConfiguration<OrderPayment>
  {
    public void Configure(EntityTypeBuilder<OrderPayment> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasRowVersion();
      builder.HasTimestamp();
      builder.Property(x => x.OrderId).IsRequired();
      builder.Property(x => x.UpdatedAt);
      builder.Property(x => x.DeletedAt);
      builder.Property(x => x.PaymentStatus);
      builder.Property(x => x.BankTransactionId);

      builder.HasOne(x => x.BankTransaction)
             .WithOne(x => x.OrderPayment)
             .HasForeignKey<OrderPayment>(x => x.BankTransactionId);


      builder.HasOne(x => x.Order)
             .WithMany(x => x.OrderPayments)
             .HasForeignKey(x => x.OrderId);
    }
  }
}