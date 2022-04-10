using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rock.Framework.Extensions;
using rock.Core.Domains.Commons;
namespace rock.Core.Data.Mappings
{
  public class BrandMap : IEntityTypeConfiguration<Brand>
  {
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasRowVersion();
      builder.HasSeoFriendly();
      builder.HasImage();
      builder.HasDescription();
      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.Name).IsRequired();
      builder.Property(x => x.ProfileId);
      builder.HasOne(x => x.Profile)
             .WithOne()
             .HasForeignKey<Brand>(x => x.ProfileId);
    }
  }
}