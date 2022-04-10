using System;
namespace rock.Core.Domains.Forms
{
  [Flags]
  public enum FormOption
  {
    None = 0,
    Financial = 1,
    Inventory = 2,
    Increase = 4,
    Decrease = 8,
    Transfer = 16,
    Manual = 32,
    Purchase = 64,
    Bill = 128,
    Shipping = 256,
    Credits = 512,
    Debits = 1024,
  }
}