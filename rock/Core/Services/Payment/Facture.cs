using System;

namespace rock.Core.Services.Payment
{
  public class Facture
  {
    public string OrderPaymentId { get; }
    public int Price { get; }

    public Facture(string orderPaymentId, int price)
    {
      OrderPaymentId = orderPaymentId;
      Price = price;
    }

  }
}
