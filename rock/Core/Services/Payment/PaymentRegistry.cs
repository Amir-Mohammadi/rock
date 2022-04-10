using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using rock.Core.Services.Orders;
using rock.Core.Services.Payment.Providers;

namespace rock.Core.Services.Payment
{
  public class PaymentGateways
  {
    public const string BANK_TEST_SEP = "banktest:sep.ir";
  }

  public class PaymentKinds
  {
    public const string CART = "cart";

  }


  public static class PaymentServiceExtention
  {
    #region maps
    private static Dictionary<string, IPaymentHandler> handlers = new Dictionary<string, IPaymentHandler>();
    private static Dictionary<string, IIPGProvider> gateways = new Dictionary<string, IIPGProvider>();
    #endregion


    public static IServiceCollection AddPayment(this IServiceCollection services)
    {

      services.AddTransient<PaymentHandlerResolver>(provider => key =>
      {
        // Register your payment handlers
        handlers[PaymentKinds.CART] = provider.GetService<ICartService>();

        return handlers[key];
      });

      services.AddTransient<IPGProviderResolver>(provider => key =>
      {
        // Register your payment gateways (IPG)
        gateways[PaymentGateways.BANK_TEST_SEP] = provider.GetService<IBankSamanIPGProvider>();

        return gateways[key];
      });
      return services;
    }
  }
}
