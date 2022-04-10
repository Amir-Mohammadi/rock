using System.ComponentModel.DataAnnotations;
namespace rock.Models.UserApi
{
  public class ResetPasswordModel
  {
    [Required]
    public string NewPassword { get; set; }
    [Required]
    public byte[] RowVersion { get; set; }
  }
}