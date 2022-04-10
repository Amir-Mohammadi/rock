using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rock.Framework.Extensions;
using rock.Core.Domains.Contents;
namespace rock.Core.Data.Mappings
{
  public class ContentMap : IEntityTypeConfiguration<Content>
  {
    public void Configure(EntityTypeBuilder<Content> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasCode();
      builder.HasSeoFriendly();
      builder.HasDescription();
      builder.HasRemoveable();
      builder.HasTimestamp();
      builder.HasRowVersion();
      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.Title).IsRequired();
      builder.Property(x => x.Body);
      builder.Property(x => x.ContentType).IsRequired();
      builder.Property(x => x.IsActive).IsRequired();
    }
  }
}