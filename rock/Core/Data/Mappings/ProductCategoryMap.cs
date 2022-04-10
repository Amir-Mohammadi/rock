using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rock.Framework.Extensions;
using rock.Core.Domains.Products;
namespace rock.Core.Data.Mappings
{
  public class ProductCategoryMap : IEntityTypeConfiguration<ProductCategory>
  {
    public void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasRowVersion();
      builder.HasTimestamp();
      builder.HasRemoveable();
      builder.HasSeoFriendly();
      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.Name).IsRequired();
      builder.Property(x => x.CatalogId).IsRequired();
      builder.Property(x => x.ParentId);
      builder.Property(x => x.IsPublished);
      builder.Property(x => x.Explanation);
      builder.HasOne(x => x.Catalog)
             .WithOne()
             .HasForeignKey<ProductCategory>(x => x.CatalogId);
      builder.HasOne(x => x.Parent)
             .WithMany(x => x.Children)
             .HasForeignKey(x => x.ParentId);
    }
  }
}