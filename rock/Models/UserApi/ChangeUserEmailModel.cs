using System;
using System.ComponentModel.DataAnnotations;
namespace rock.Models.UserApi
{
  public class ChangeUserEmailModel
  {
    [Required, EmailAddress]
    public string Email { get; set; }
    [Required]
    public byte[] RowVersion { get; set; }
  }
}