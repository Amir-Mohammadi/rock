using System;
using System.ComponentModel.DataAnnotations;
namespace rock.Models.ShopApi
{
  public class ShopStuffPriceAdjustmentModel
  {
    [Required]
    public double Price { get; set; }
    [Required]
    public int ColorId { get; set; }
  }
}