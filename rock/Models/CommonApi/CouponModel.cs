using System;
using System.ComponentModel.DataAnnotations;

namespace rock.Models.CommonApi
{
  public class CouponModel
  {
    public int Id { get; set; }
    [Required]
    public string CouponCode { get; set; }
    [Required]
    public int? MaxQuantities { get; set; }
    [Required]
    public double Value { get; set; }
    [Required]
    public DateTime ExpiryDate { get; set; }

    public int MaxQuantitiesPerUser { get; set; }

    public bool Active { get; set; }

    public byte[] RowVersion { get; set; }
  }
}