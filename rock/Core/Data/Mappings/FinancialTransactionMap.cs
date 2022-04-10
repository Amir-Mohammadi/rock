using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rock.Core.Domains.Financial;
using rock.Framework.Extensions;

namespace rock.Core.Data.Mappings
{
  public class FinancialTransactionMap : IEntityTypeConfiguration<FinancialTransaction>
  {
    public void Configure(EntityTypeBuilder<FinancialTransaction> builder)
    {
      builder.HasKey(x => x.Id);
      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.Factor).IsRequired();
      builder.Property(x => x.Amount).HasPrecision(25, 9).IsRequired();
      builder.Property(x => x.FinancialAccountId).IsRequired();
      builder.Property(x => x.CreatedAt).IsRequired();
      builder.Property(x => x.DocumentId).IsRequired();

      builder.HasOne(x => x.Document)
             .WithMany(x => x.FinancialTransactions)
             .HasForeignKey(x => x.DocumentId);
      
      builder.HasOne(x => x.Account)
             .WithMany(x => x.FinancialTransactions)
             .HasForeignKey(x => x.FinancialAccountId);

      builder.HasRowVersion();
      builder.HasRemoveable();


    }
  }
}