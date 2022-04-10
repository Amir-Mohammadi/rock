using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rock.Framework.Extensions;
using rock.Core.Domains.Profiles;
namespace rock.Core.Data.Mappings
{
  public class PersonProfileMap : IEntityTypeConfiguration<PersonProfile>
  {
    public void Configure(EntityTypeBuilder<PersonProfile> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasRowVersion();
      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.NationalCode);
      builder.Property(x => x.EconomicCode);
      builder.Property(x => x.Birthdate);
      builder.Property(x => x.FatherName);
      builder.Property(x => x.FirstName);
      builder.Property(x => x.LastName);
      builder.Property(x => x.Gender);
      builder.Property(x => x.PictureId);
      builder.Property(x => x.ProfileId);
      builder.HasOne(x => x.Picture)
            .WithMany()
            .HasForeignKey(x => x.PictureId);
      builder.HasOne(x => x.Profile)
             .WithOne(x => x.PersonProfile)
             .HasForeignKey<PersonProfile>(x => x.ProfileId);
    }
  }
}