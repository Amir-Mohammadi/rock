using System;

namespace rock.Models.CustomerApi
{
  public class OrderCheckoutBankBillModel
  {
    public DateTime BillDateTime { get; set; }
    public string OrderNum { get; set; }
    public string PaymentNum { get; set; }
    public string RefNum { get; set; }
  }
}