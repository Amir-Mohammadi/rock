using System;
using System.ComponentModel.DataAnnotations;

namespace rock.Models.ProductApi
{
  public class AnswerProductQuestionModel
  {
    [Required]
    [StringLength(255)]
    public string Answer { get; set; }
  }
}
