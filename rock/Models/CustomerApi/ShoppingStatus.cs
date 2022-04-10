using System;

namespace rock.Models.CustomerApi
{
  public enum ShoppingStatus
  {
    InProgress,
    AcceptByProvider,
    Sending,
    Completed,
    ReturnedByUser,
    Canceled,
  }
}
