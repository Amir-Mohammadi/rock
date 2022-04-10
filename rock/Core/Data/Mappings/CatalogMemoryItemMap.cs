using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rock.Framework.Extensions;
using rock.Core.Domains.Catalogs;
namespace rock.Core.Data.Mappings
{
  public class CatalogMemoryItemMap : IEntityTypeConfiguration<CatalogMemoryItem>
  {
    public void Configure(EntityTypeBuilder<CatalogMemoryItem> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasRowVersion();
      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.Value).IsRequired();
      builder.Property(x => x.ExtraKey);
      builder.Property(x => x.CatalogItemId).IsRequired();
      builder.Property(x => x.CatalogMemoryId).IsRequired();
      builder.HasOne(x => x.CatalogMemory)
             .WithMany(x => x.Items)
             .HasForeignKey(x => x.CatalogMemoryId);
      builder.HasOne(x => x.CatalogItem)
             .WithMany(x => x.CatalogMemoryItems)
             .HasForeignKey(x => x.CatalogItemId);
    }
  }
}