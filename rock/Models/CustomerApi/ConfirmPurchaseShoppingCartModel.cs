using System.ComponentModel.DataAnnotations;

namespace rock.Models.CustomerApi
{
  public class ConfirmPurchaseShoppingCartModel
  {
    [Required]
    public int OrderId { get; set; }

    public int TransactionReferenceID { get; set; }

  }
}
