using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rock.Framework.Extensions;
using rock.Core.Domains.Profiles;
namespace rock.Core.Data.Mappings
{
  public class CompanyProfileMap : IEntityTypeConfiguration<CompanyProfile>
  {
    public void Configure(EntityTypeBuilder<CompanyProfile> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasRowVersion();
      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.RegistrationCode).IsRequired();
      builder.Property(x => x.ProfileId).IsRequired();
      builder.HasOne(x => x.Profile)
        .WithOne(x => x.CompanyProfile)
        .HasForeignKey<CompanyProfile>(x => x.ProfileId);
    }
  }
}