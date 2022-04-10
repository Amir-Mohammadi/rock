using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rock.Core.Domains.Documents;
using rock.Framework.Extensions;
namespace rock.Core.Data.Mappings
{
  public class DocumentMap : IEntityTypeConfiguration<Document>
  {
    public void Configure(EntityTypeBuilder<Document> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasRowVersion();
      builder.HasDescription();
      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.FormId).IsRequired();
      builder.Property(x => x.UserId).IsRequired();
      builder.Property(x => x.CreatedAt).IsRequired();
      builder.HasOne(x => x.User)
             .WithMany()
             .HasForeignKey(x => x.UserId);
      builder.HasOne(x => x.Form)
             .WithMany()
             .HasForeignKey(x => x.FormId);
    }
  }
}