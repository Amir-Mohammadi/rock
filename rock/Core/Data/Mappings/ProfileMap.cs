using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rock.Framework.Extensions;
using rock.Core.Domains.Profiles;
using rock.Core.Domains.Users;
namespace rock.Core.Data.Mappings
{
  public class ProfileMap : IEntityTypeConfiguration<Profile>
  {
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasRowVersion();
      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.Phone);
      builder.Property(x => x.Email);
      builder.Property(x => x.Type).IsRequired();
      builder.Property(x => x.CityId);
      builder.HasOne(x => x.City)
      .WithMany()
      .HasForeignKey(x => x.CityId);
    }
  }
}