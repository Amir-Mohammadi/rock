using System;

namespace rock.Framework.Models
{
  public interface IRemovable
  {
    DateTime? DeletedAt { get; set; }
  }
}