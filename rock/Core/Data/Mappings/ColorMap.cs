using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using rock.Framework.Extensions;
using rock.Core.Domains.Commons;

namespace rock.Core.Data.Mappings
{
  public class ColorMap : IEntityTypeConfiguration<Color>
  {
    public void Configure(EntityTypeBuilder<Color> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasRowVersion();
      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.Name).IsRequired();
      builder.Property(x => x.Code).IsRequired();
    }
  }
}