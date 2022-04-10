using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using rock.Framework.Autofac;
using rock.Core.Services.Payment;
using rock.Core.Domains.Payment;

namespace rock.Core.Services.Payment
{
  public delegate IIPGProvider IPGProviderResolver(string gateway);
  public interface IIPGProvider
  {


    Task<string> Purchase(Facture facture, BankTransaction bankTransaction, CancellationToken cancellationToken);

    Task<bool> Verify(Dictionary<string, string> requestParameters, BankTransaction bankTransactionId, CancellationToken cancellationToken);

    BankInfo Parse(Dictionary<string, string> requestParameters);

  }
}
