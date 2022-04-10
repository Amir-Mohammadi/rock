using System;

namespace rock.Framework.Models
{
  public interface ITimestamp
  {
    DateTime CreatedAt { get; set; }
    DateTime? UpdatedAt { get; set; }

  }
}