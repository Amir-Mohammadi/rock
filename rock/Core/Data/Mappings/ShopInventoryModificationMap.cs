using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rock.Framework.Extensions;
using rock.Core.Domains.Warehousing;
namespace rock.Core.Data.Mappings
{
  public class ShopInventoryModificationMap : IEntityTypeConfiguration<ShopInventoryModification>
  {
    public void Configure(EntityTypeBuilder<ShopInventoryModification> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasRowVersion();
      builder.HasDescription();
      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.DocumentId).IsRequired();
      builder.HasOne(x => x.Document)
             .WithMany()
             .HasForeignKey(x => x.DocumentId);
    }
  }
}