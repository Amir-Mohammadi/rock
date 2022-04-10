using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using rock.Framework.Extensions;
using rock.Core.Domains.Catalogs;

namespace rock.Core.Data.Mappings
{
  public class CatalogMap : IEntityTypeConfiguration<Catalog>
  {
    public void Configure(EntityTypeBuilder<Catalog> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasRowVersion();
      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.CreatedAt).IsRequired();

      builder.HasMany(x => x.Items)
             .WithOne(x => x.Catalog)
             .HasForeignKey(x => x.CatalogId);
    }
  }
}