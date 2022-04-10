using System.ComponentModel.DataAnnotations;

namespace rock.Models.CommonApi
{
  public class CreateTransportationModel
  {
    [Required]
    public int FromCityId { get; set; }
    [Required]
    public int ToCityId { get; set; }
    [Required]
    public int Distance { get; set; }
    [Required]
    public int Cost { get; set; }
    public string Description { get; set; }
  }
}