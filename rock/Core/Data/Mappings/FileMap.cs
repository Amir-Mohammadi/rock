using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rock.Core.Domains;
using rock.Core.Domains.Files;
using rock.Framework.Extensions;
namespace rock.Core.Data.Mappings
{
  public class FileMap : IEntityTypeConfiguration<File>
  {
    public void Configure(EntityTypeBuilder<File> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasRemoveable();
      builder.HasTimestamp();
      builder.HasRowVersion();
      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.ThreadId);
      builder.Property(x => x.FileType).IsRequired();
      builder.Property(x => x.Access).IsRequired();
      builder.Property(x => x.FileName).IsRequired();
      builder.Property(x => x.FileStream).IsRequired();
      builder.Property(x => x.OwnerGroup);
      builder.HasOne(x => x.Thread)
             .WithOne()
             .HasForeignKey<File>(x => x.ThreadId);
    }
  }
}