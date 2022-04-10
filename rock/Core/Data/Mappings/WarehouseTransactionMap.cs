using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rock.Framework.Extensions;
using rock.Core.Domains.Warehousing;
namespace rock.Core.Data.Mappings
{
  public class WarhouseTransactionMap : IEntityTypeConfiguration<WarehouseTransaction>
  {
    public void Configure(EntityTypeBuilder<WarehouseTransaction> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasRowVersion();
      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.Amount).HasPrecision(25, 9).IsRequired();
      builder.Property(x => x.WarehouseId).IsRequired();
      builder.Property(x => x.ColorId).IsRequired();
      builder.Property(x => x.ProductId).IsRequired();
      builder.HasOne(x => x.Document)
             .WithMany(x => x.WarehouseTransactions)
             .HasForeignKey(x => x.DocumentId);
      builder.HasOne(x => x.Warehouse)
             .WithMany()
             .HasForeignKey(x => x.WarehouseId);
      builder.HasOne(x => x.Product)
             .WithMany()
             .HasForeignKey(x => x.ProductId);
      builder.HasOne(x => x.Color)
             .WithMany()
             .HasForeignKey(x => x.ColorId);
      builder.HasOne(x => x.ProductColor)
             .WithMany()
             .HasForeignKey(x => new { x.ProductId, x.ColorId });
    }
  }
}