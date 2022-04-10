using System;
using System.Collections.Generic;
using rock.Core.Domains.Orders;
namespace rock.Models.OrderApi
{
  public class OrderDetailModel
  {
    public int Id { get; set; }
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public string Province { get; set; }
    public string City { get; set; }
    public string Address { get; set; }
    public string PostalCode { get; set; }
    public string PhoneNumber { get; set; }
    public string TelNumber { get; set; }
    public string PaymentTrackingCode { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public byte[] RowVersion { get; set; }
  }
}