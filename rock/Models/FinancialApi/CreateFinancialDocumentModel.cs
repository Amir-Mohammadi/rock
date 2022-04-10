using System.ComponentModel.DataAnnotations;

namespace rock.Models.FinancialApi
{
  public class CreateDocumentModel
  {
    [Required]
    public int FormId { get; set; }
  }
}