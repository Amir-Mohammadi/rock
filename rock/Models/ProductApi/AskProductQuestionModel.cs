using System;
using System.ComponentModel.DataAnnotations;
namespace rock.Models.ProductApi
{
  public class AskProductQuestionModel
  {
    [Required]
    [StringLength(255)]
    public string Question { get; set; }
    public byte[] RowVersion { get; set; }
  }
}