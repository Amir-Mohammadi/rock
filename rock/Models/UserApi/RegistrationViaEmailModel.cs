using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using rock.Core.Services.Users;
namespace rock.Models.UserApi
{
  public class RegistrationViaEmailModel
  {
    [Required, EmailAddress]
    public string Email { get; set; }
    [Required, StringLength(64, MinimumLength = 8)]
    public string Password { get; set; }
    [StringLength(64, MinimumLength = 2)]
    public string FirstName { get; set; }
    [StringLength(64, MinimumLength = 2)]
    public string LastName { get; set; }
  }
}