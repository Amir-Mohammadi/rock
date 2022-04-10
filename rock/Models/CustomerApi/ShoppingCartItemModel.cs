using System;
using System.ComponentModel.DataAnnotations;
using rock.Core.Domains.Products;
using rock.Models.MarketApi;
namespace rock.Models.CustomerApi
{
  public class ShoppingCartItemModel
  {
    [Required]
    public int ProductId { get; set; }
    [Required]
    public int Amount { get; set; }
    [Required]
    public int ColorId { get; set; }
  }
}