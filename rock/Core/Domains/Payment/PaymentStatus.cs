namespace rock.Core.Domains.Payment
{
  public enum PaymentStatus
  {
    Created = 0,
    Pending = 1,
    Returned = 2,
    Cancelled = 3,
    Failed = 4,
    Done = 5
  }
}