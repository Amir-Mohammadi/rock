using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rock.Framework.Extensions;
using rock.Core.Domains.Catalogs;
namespace rock.Core.Data.Mappings
{
  public class CatalogMemoryMap : IEntityTypeConfiguration<CatalogMemory>
  {
    public void Configure(EntityTypeBuilder<CatalogMemory> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasRowVersion();
      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.CatalogId).IsRequired();
      builder.HasOne(x => x.Catalog)
             .WithMany(x => x.CatalogMemories)
             .HasForeignKey(x => x.CatalogId);
    }
  }
}