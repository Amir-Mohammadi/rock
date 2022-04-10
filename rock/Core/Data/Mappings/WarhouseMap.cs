using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rock.Framework.Extensions;
using rock.Core.Domains.Warehousing;
namespace rock.Core.Data.Mappings
{
  public class WarhouseMap : IEntityTypeConfiguration<Warehouse>
  {
    public void Configure(EntityTypeBuilder<Warehouse> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasRowVersion();
      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.Name).IsRequired();
    }
  }
}