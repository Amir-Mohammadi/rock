using System;
using System.ComponentModel.DataAnnotations;

namespace rock.Models.PaymentApi
{
  public class CreatePaymentGatewayModel
  {
    [Required]
    public string Gateway { get; set; }

    [Required]
    public string ImageAlt { get; set; }

    [Required]
    public string ImageTitle { get; set; }

    [Required]
    public Guid ImageId { get; set; }

    [Required]
    public int FinancialAccountId { get; set; }

    public bool IsDefault { get; set; }
  }
}