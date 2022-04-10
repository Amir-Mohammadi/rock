using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rock.Core.Domains.Users;
using rock.Framework.Extensions;
namespace rock.Core.Data.Mappings
{
  public class UserMap : IEntityTypeConfiguration<User>
  {
    public void Configure(EntityTypeBuilder<User> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasRowVersion();
      builder.HasTimestamp();
      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.Role);
      builder.Property(x => x.ProfileId);
      builder.Property(x => x.Password);
      builder.Property(x => x.Enabled);
      builder.HasOne(x => x.Profile)
        .WithOne()
        .HasForeignKey<User>(x => x.ProfileId);
    }
  }
}