using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rock.Framework.Extensions;
using rock.Core.Domains.Catalogs;
namespace rock.Core.Data.Mappings
{
  public class CatalogItemMap : IEntityTypeConfiguration<CatalogItem>
  {
    public void Configure(EntityTypeBuilder<CatalogItem> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasRowVersion();
      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.Type).IsRequired();
      builder.Property(x => x.Order).IsRequired();
      builder.Property(x => x.IsMain).IsRequired();
      builder.Property(x => x.HasMultiple).IsRequired();
      builder.Property(x => x.Value).IsRequired();
      builder.Property(x => x.ShowInFilter).IsRequired();
      builder.Property(x => x.ReferenceId);
      builder.HasOne(x => x.Reference)
             .WithMany(x => x.Children)
             .HasForeignKey(x => x.ReferenceId);
      builder.HasOne(x => x.Catalog)
             .WithMany(x => x.Items)
             .HasForeignKey(x => x.CatalogId);
    }
  }
}