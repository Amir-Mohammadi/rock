using System;
using System.Collections.Generic;
using rock.Core.Domains.Users;
using rock.Framework.Models;
namespace rock.Core.Domains.Threads
{
  public class ThreadActivity : IEntity, ITimestamp, IRemovable
  {
    public int Id { get; set; }
    public ThreadActivityType Type { get; set; }
    public string Payload { get; set; }
    public int? ReferenceId { get; set; }
    public int ThreadId { get; set; }
    public int UserId { get; set; }
    public int? PublisherId { get; set; }
    public DateTime? PublishAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Thread Thread { get; set; }
    public virtual ThreadActivity Reference { get; set; }
    public virtual ICollection<ThreadActivity> ThreadActivityItems { get; set; }
    public virtual User User { get; set; }
    public virtual User Publisher { get; set; }
  }
}