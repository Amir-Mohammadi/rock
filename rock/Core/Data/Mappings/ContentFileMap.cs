using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rock.Core.Domains.Contents;
using rock.Framework.Extensions;
namespace rock.Core.Data.Mappings
{
  public class ContentFileMap : IEntityTypeConfiguration<ContentFile>
  {
    public void Configure(EntityTypeBuilder<ContentFile> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasRowVersion();
      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.FileId).IsRequired();
      builder.Property(x => x.ContentId).IsRequired();
      builder.Property(x => x.Title);
      builder.Property(x => x.ImageAlt);
      builder.Property(x => x.ContentId).IsRequired();
      builder.HasOne(x => x.File)
             .WithOne()
             .HasForeignKey<ContentFile>(x => x.FileId);
      builder.HasOne(x => x.Content)
             .WithMany(x => x.ContentFiles)
             .HasForeignKey(x => x.ContentId);
    }
  }
}