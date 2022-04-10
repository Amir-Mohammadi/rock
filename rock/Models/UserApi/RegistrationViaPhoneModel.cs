using System;
using System.ComponentModel.DataAnnotations;
namespace rock.Models.UserApi
{
  public class RegistrationViaPhoneModel
  {
    [Required, Phone]
    public string Phone { get; set; }
    [StringLength(64, MinimumLength = 2)]
    public string FirstName { get; set; }
    [StringLength(64, MinimumLength = 2)]
    public string LastName { get; set; }
  }
}