using System;

namespace rock.Core.Common
{
  public interface IStatefullObject
  {
    ObjectState State { get; set; }
  }

  public enum ObjectState
  {
    None,
    Added,
    Deleted,
    Updated,
  }
}
