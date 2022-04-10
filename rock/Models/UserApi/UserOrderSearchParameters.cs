using System;
using rock.Filters;
using Microsoft.AspNetCore.Mvc;
using rock.Core.Domains.Orders;
namespace rock.Models.UserApi
{
  public class UserOrderSearchParameters
  {
    [FromQuery(Name = "status")]
    public OrderItemStatusType? Status { get; set; }
  }
}