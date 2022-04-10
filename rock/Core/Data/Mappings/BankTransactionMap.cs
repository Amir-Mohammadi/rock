using Microsoft.EntityFrameworkCore;
using rock.Core.Domains.Payment;
using rock.Framework.Extensions;

namespace rock.Core.Data.Mappings
{
  public class BankTransactionMap : IEntityTypeConfiguration<BankTransaction>
  {
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<BankTransaction> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasRowVersion();
      builder.HasTimestamp();
      builder.Property(x => x.OrderPaymentId);
      builder.Property(x => x.PaymentKind);
      builder.Property(x => x.PaymentGateway);
      builder.Property(x => x.TerminalId);
      builder.Property(x => x.CardNo);
      builder.Property(x => x.BankParameter1);
      builder.Property(x => x.BankParameter2);
      builder.Property(x => x.BankParameter3);
      builder.Property(x => x.BankParameter4);
      builder.Property(x => x.Amount).IsRequired();
      builder.Property(x => x.State).IsRequired();
      builder.Property(x => x.UserId).IsRequired();
      builder.HasOne(x => x.User)
             .WithMany(x => x.BankTransactions)
             .HasForeignKey(x => x.UserId);
    }
  }
}