using System;
namespace rock.Core.Domains.Orders
{
  public enum OrderItemStatusType
  {
    Created = 0,
    InProgress = 1,
    Prepared = 2,
    Sended = 3,
    Delivered = 4,
    Returned = 5,
    Cancelled = 6
  }
}