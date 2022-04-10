using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using rock.Framework.Extensions;
using rock.Core.Domains.Profiles;

namespace rock.Core.Data.Mappings
{
  public class ProfileAddressMap : IEntityTypeConfiguration<ProfileAddress>
  {
    public void Configure(EntityTypeBuilder<ProfileAddress> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasRowVersion();
      builder.HasDescription();
      builder.HasRemoveable();
      builder.Property(x => x.Phone).IsRequired();
      builder.Property(x => x.ProfileId).IsRequired();
      builder.Property(x => x.CityId).IsRequired();
      builder.Property(x => x.AddressOwnerName).IsRequired();
      builder.Property(x => x.IsDefault).IsRequired();
      builder.HasOne(x => x.City).WithMany().HasForeignKey(x => x.CityId);
      builder.HasOne(x => x.Profile).WithMany(x => x.Addresses).HasForeignKey(x => x.ProfileId);

    }
  }
}