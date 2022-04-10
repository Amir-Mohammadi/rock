using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rock.Framework.Extensions;
using rock.Core.Domains.Products;
using System.Collections.Generic;
using System;
namespace rock.Core.Data.Mappings
{
  public class ProductCategoryBrandMap : IEntityTypeConfiguration<ProductCategoryBrand>
  {
    public void Configure(EntityTypeBuilder<ProductCategoryBrand> builder)
    {
      builder.HasKey(x => new { x.ProductCategoryId, x.BrandId });
      builder.HasRowVersion();
      builder.Property(x => x.BrandId).IsRequired();
      builder.Property(x => x.ProductCategoryId).IsRequired();
      
      builder.HasOne(x => x.Brand)
             .WithMany(x => x.ProductCategoryBrands)
             .HasForeignKey(x => x.BrandId);
      
      builder.HasOne(x => x.ProductCategory)
             .WithMany(x => x.ProductCategoryBrands)
             .HasForeignKey(x => x.ProductCategoryId);
    }
  }
}