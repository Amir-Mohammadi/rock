using System;
using System.ComponentModel.DataAnnotations;

namespace rock.Models.CustomerApi
{
  public class PurchaseShoppingCartModel
  {
    /// <summary>
    /// This is the address going to redirected after finishing the payment
    /// </summary>
    [Required, Url]
    public Uri CallbackAddress { get; set; }
  }
}
