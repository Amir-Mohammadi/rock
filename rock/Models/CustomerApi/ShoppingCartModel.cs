using System;
using System.Collections.Generic;
using rock.Core.Domains.Orders;
using rock.Models.MarketApi;
using rock.Models.UserApi;

namespace rock.Models.CustomerApi
{
  public class ShoppingCartModel
  {
    public CartModel Cart { get; set; }
    public UserAddressModel UserAddressModel { get; set; }
  }
}
