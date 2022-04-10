using System;
namespace rock.Core.Domains.Users
{
  public enum UserRole : int
  {
    None = 0,
    Customer = 1,
    Merchant = 2,
    Support = 3,
    ContentProvider = 4,
    Accountant = 5,
    MarketManager = 6,
    FinancialManager = 7,
    Admin = 8,
    MarketAgent = 9,
  }
}