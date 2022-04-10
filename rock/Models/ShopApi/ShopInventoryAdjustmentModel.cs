using System.ComponentModel.DataAnnotations;
namespace rock.Models.ShopApi
{
  public class ShopInventoryAdjustmentModel
  {
    [Required]
    public int Quantity { get; set; }
    [Required]
    public int ColorId { get; set; }
  }
}