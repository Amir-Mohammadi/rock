using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using rock.Framework.Extensions;
using rock.Core.Domains.Financial;

namespace rock.Core.Data.Mappings
{
  public class BankMap : IEntityTypeConfiguration<Bank>
  {
    public void Configure(EntityTypeBuilder<Bank> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasRowVersion();
      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.Name).IsRequired();
    }
  }
}