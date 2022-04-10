using System;
using rock.Core.Domains.Payment;

namespace rock.Core.StaticData
{
  public class PaymentGateways
  {
    private static PaymentGateway samanGateway = new()
    {
      Gateway = "banktest:sep.ir",
      FinancialAccountId = 14,
      ImageId = new Guid("245D7A92-3043-4776-925C-E1C415694949"),
      ImageAlt = "درگاه پرداخت بانک سامان",
      ImageTitle = "درگاه پرداخت بانک سامان",
    };
    public static PaymentGateway SamanGateway { get => samanGateway; }

  }
}