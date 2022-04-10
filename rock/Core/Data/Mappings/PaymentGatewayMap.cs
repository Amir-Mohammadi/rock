using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rock.Core.Domains.Payment;
using rock.Framework.Extensions;

namespace rock.Core.Data.Mappings
{
  public class PaymentGatewayMap : IEntityTypeConfiguration<PaymentGateway>
  {
    public void Configure(EntityTypeBuilder<PaymentGateway> builder)
    {
      builder.HasKey(x => x.Gateway);
      builder.Property(x => x.Gateway).IsRequired();
      builder.Property(x => x.FinancialAccountId).IsRequired();
      builder.Property(x => x.CreatedAt).IsRequired();
      builder.Property(x => x.DeletedAt);
      builder.Property(x => x.IsDefault).IsRequired();
      builder.HasOne(x => x.FinancialAccount)
             .WithMany(x => x.PaymentGateways)
             .HasForeignKey(x => x.FinancialAccountId);

      builder.HasRemoveable();
      builder.HasImage();
      builder.HasRowVersion();
    }
  }
}