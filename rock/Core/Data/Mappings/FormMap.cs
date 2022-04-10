using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rock.Framework.Extensions;
using rock.Core.Domains.Forms;
namespace rock.Core.Data.Mappings
{
  public class FormMap : IEntityTypeConfiguration<Form>
  {
    public void Configure(EntityTypeBuilder<Form> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasRowVersion();
      builder.HasDescription();
      builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
      builder.Property(x => x.Title).IsRequired();
      builder.Property(x => x.FormOptions).IsRequired();
    }
  }
}