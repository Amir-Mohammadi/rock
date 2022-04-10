using System.ComponentModel.DataAnnotations;
using rock.Core.Domains.Users;

namespace rock.Models.UserApi
{
  public class CreateUserModel
  {

    public string FirstName { get; set; }

    public string LastName { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public UserRole UserRole { get; set; }

    [Required]
    [Phone]
    public string Phone { get; set; }
  }
}