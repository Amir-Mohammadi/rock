using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using rock.Framework.Extensions;
using rock.Core.Domains.Commons;

namespace rock.Core.Data.Mappings
{
  public class CurrencyMap : IEntityTypeConfiguration<Currency>
  {
    public void Configure(EntityTypeBuilder<Currency> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasRowVersion();
      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.Name).IsRequired();
      builder.Property(x => x.Symbol).IsRequired();
      builder.Property(x => x.Ratio).IsRequired();

    }
  }
}