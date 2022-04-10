using System;
using System.ComponentModel.DataAnnotations;
using rock.Core.Domains.Orders;

namespace rock.Models.OrderApi
{
  public class OrderStatusModel
  {
    public int Id { get; set; }
    [Required]
    public OrderItemStatusType Type { get; set; }
    [Required]
    public int OrderItemId { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
