using System.Collections.Generic;
using rock.Core.Domains.Users;

namespace rock.OAuth
{
  public static class AuthGateway
  {
    public static readonly Dictionary<AuthGatewayType, UserRole> Map = new()
    {
      [AuthGatewayType.Admin] = UserRole.Admin | UserRole.Accountant | UserRole.ContentProvider | UserRole.FinancialManager,
      [AuthGatewayType.Seller] = UserRole.Admin | UserRole.Merchant | UserRole.Merchant,

    };

    public static bool CheckAuthGateway(AuthGatewayType authGateway, UserRole userRole)
    {
      var map = Map;
      var value = map[authGateway];
      return (value & userRole) == userRole;
    }
  }
}
