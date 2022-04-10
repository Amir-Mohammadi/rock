using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rock.Framework.Extensions;
using rock.Core.Domains.Financial;

namespace rock.Core.Data.Mappings
{
  public class FinancialAccountMap : IEntityTypeConfiguration<FinancialAccount>
  {
    public void Configure(EntityTypeBuilder<FinancialAccount> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasRowVersion();
      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.Title).IsRequired();
      builder.Property(x => x.ProfileId).IsRequired();
      builder.Property(x => x.BankId);
      builder.Property(x => x.CurrencyId).IsRequired();
      builder.Property(x => x.No);
      builder.Property(x => x.Type).IsRequired();

      builder.HasOne(x => x.Bank)
             .WithMany(x => x.FinancialAccounts)
             .HasForeignKey(x => x.BankId);

      builder.HasOne(x => x.Currency)
             .WithMany()
             .HasForeignKey(x => x.CurrencyId);

      builder.HasOne(x => x.Profile)
             .WithOne(x => x.FinancialAccount)
             .HasForeignKey<FinancialAccount>(x => x.ProfileId);

    }
  }
}