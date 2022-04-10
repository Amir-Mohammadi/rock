using System;

namespace rock.Core.Domains.Payment
{
  public enum BankTransactionState
  {
    Waiting,
    Verified,
    Ok,
    Failed
  }
}
