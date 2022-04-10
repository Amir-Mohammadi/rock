using System.ComponentModel.DataAnnotations;

namespace rock.Models.CommonApi
{
  public class UpdateTransportationModel
  {
    public string Description { get; set; }

    [Required]
    public byte[] RowVersion { get; set; }
  }
}