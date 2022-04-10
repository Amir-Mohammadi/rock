using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using rock.Core.Domains.Threads;
using rock.Framework.Extensions;
namespace rock.Core.Data.Mappings
{
  public class ThreadActivityMap : IEntityTypeConfiguration<ThreadActivity>
  {
    public void Configure(EntityTypeBuilder<ThreadActivity> builder)
    {
      builder.HasKey(x => x.Id);
      builder.HasRemoveable();
      builder.HasTimestamp();
      builder.Property(x => x.Id).IsRequired();
      builder.Property(x => x.Payload);
      builder.Property(x => x.Type).IsRequired();
      builder.Property(x => x.UserId).IsRequired();
      builder.Property(x => x.PublisherId);
      builder.Property(x => x.PublishAt);
      builder.Property(x => x.ThreadId).IsRequired();
      builder.Property(x => x.ReferenceId);
      builder.HasOne(x => x.User)
             .WithMany(x => x.ThreadActivities)
             .HasForeignKey(f => f.UserId)
             .OnDelete(DeleteBehavior.Restrict);
      builder.HasOne(x => x.Thread)
             .WithMany(x => x.Activities)
             .HasForeignKey(x => x.ThreadId)
             .OnDelete(DeleteBehavior.Restrict);
      builder.HasOne(x => x.Reference)
             .WithMany(x => x.ThreadActivityItems)
             .HasForeignKey(x => x.ReferenceId)
             .OnDelete(DeleteBehavior.NoAction);
      builder.HasOne(x => x.Publisher)
             .WithMany(x => x.PublishedThreadActivities)
             .HasForeignKey(f => f.PublisherId)
             .OnDelete(DeleteBehavior.Restrict);
    }
  }
}