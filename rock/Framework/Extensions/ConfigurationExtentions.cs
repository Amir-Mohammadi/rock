using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rock.Framework.Models;
namespace rock.Framework.Extensions
{
  public static class ConfigurationExtentions
  {
    public static void HasRowVersion<TEntity>(this EntityTypeBuilder<TEntity> builder)
         where TEntity : class, IHasRowVersion
    {
      builder.Property(x => x.RowVersion).IsRowVersion().IsRequired();
    }
    public static void HasDescription<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IHasDescription
    {
      builder.Property(x => x.Description);
    }
    public static void HasCode<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IHasCode
    {
      builder.Property(x => x.Code).IsRequired().HasMaxLength(50);
      builder.HasIndex(x => x.Code).IsUnique();
    }
    public static void HasRemoveable<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IRemovable
    {
      builder.Property(x => x.DeletedAt);
    }
    public static void HasImage<TEntity>(this EntityTypeBuilder<TEntity> builder, bool IsRequired = true)
        where TEntity : class, IHasImage
    {
      builder.Property(x => x.ImageId).IsRequired(IsRequired);
      builder.Property(x => x.ImageTitle).IsRequired(IsRequired);
      builder.Property(x => x.ImageAlt).IsRequired(IsRequired);
      builder.HasOne(x => x.Image)
             .WithOne()
             .HasForeignKey<TEntity>(x => x.ImageId);
    }
    public static void HasTimestamp<TEntity>(this EntityTypeBuilder<TEntity> builder)
    where TEntity : class, ITimestamp
    {
      builder.Property(x => x.CreatedAt).IsRequired();
      builder.Property(x => x.UpdatedAt);
    }
    public static void HasSeoFriendly<TEntity>(this EntityTypeBuilder<TEntity> builder)
         where TEntity : class, ISeoFriendly
    {
      builder.Property(x => x.UrlTitle);
      builder.Property(x => x.BrowserTitle);
      builder.Property(x => x.MetaDescription);
    }
  }
}