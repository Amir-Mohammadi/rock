using System;
using System.Collections.Generic;
using rock.Framework.Models;
namespace rock.Core.Domains.Threads
{
  public class Thread : IEntity, IRemovable
  {
    public Thread()
    {
      this.Activities = new HashSet<ThreadActivity>();
    }
    public int Id { get; set; }
    public DateTime? DeletedAt { get; set; }
    public virtual ICollection<ThreadActivity> Activities { get; set; }
    public byte[] RowVersion { get; set; }
  }
}