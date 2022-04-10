using System.ComponentModel.DataAnnotations;
namespace rock.Models.UserApi
{
  public class ChangePasswordModel
  {
    [Required]
    public string OldPassword { get; set; }
    [Required]
    public string NewPassword { get; set; }
  }
}