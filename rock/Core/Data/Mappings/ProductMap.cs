using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rock.Core.Domains.Products;
using rock.Framework.Extensions;
namespace rock.Core.Data.Mappings
{
  public class ProductMap : IEntityTypeConfiguration<Product>
  {
    public void Configure(EntityTypeBuilder<Product> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasRowVersion();
      builder.HasRemoveable();
      builder.HasSeoFriendly();
      builder.Property(x => x.Id).IsRequired().ValueGeneratedOnAdd();
      builder.Property(x => x.ProductCategoryId).IsRequired();
      builder.Property(x => x.CatalogMemoryId).IsRequired();
      builder.Property(x => x.BrandId).IsRequired();
      builder.Property(x => x.Name).IsRequired();
      builder.Property(x => x.BriefDescription);
      builder.Property(x => x.PreviewProductImageId);
      builder.Property(x => x.ThreadId).IsRequired();
      builder.Property(x => x.DefaultColorId);
      builder.HasOne(x => x.PreviewProductImage)
             .WithOne()
             .HasForeignKey<Product>(x => x.PreviewProductImageId);
      builder.HasOne(x => x.DefaultProductColor)
             .WithOne()
             .HasForeignKey<Product>(x => new { x.Id, x.DefaultColorId });
      builder.HasOne(x => x.CatalogMemory)
             .WithMany()
             .HasForeignKey(x => x.CatalogMemoryId);
      builder.HasOne(x => x.ProductCategory)
             .WithMany(x => x.Products)
             .HasForeignKey(x => x.ProductCategoryId);
      builder.HasOne(x => x.Brand)
             .WithMany()
             .HasForeignKey(x => x.BrandId);
      builder.HasMany(x => x.ProductColors)
             .WithOne(x => x.Product)
             .HasForeignKey(x => x.ProductId);
      builder.HasOne(x => x.Thread)
             .WithMany()
             .HasForeignKey(x => x.ThreadId);
    }
  }
}