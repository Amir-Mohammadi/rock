using System;
using System.Collections.Generic;
using rock.Framework.Models;
using rock.Core.Domains.Threads;
using rock.Core.Domains.Files;
using rock.Core.Domains.Profiles;
using rock.Core.Domains.Payment;

namespace rock.Core.Domains.Users
{
  public class User : IEntity, ITimestamp
  {
    public int Id { get; set; }
    public int ProfileId { get; set; }
    public UserRole Role { get; set; }
    public string Password { get; set; }
    public bool Enabled { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Profile Profile { get; set; }
    public virtual ICollection<ThreadActivity> ThreadActivities { get; set; }
    public virtual ICollection<ThreadActivity> PublishedThreadActivities { get; set; }
    public virtual ICollection<BankTransaction> BankTransactions { get; set; }
  }
}