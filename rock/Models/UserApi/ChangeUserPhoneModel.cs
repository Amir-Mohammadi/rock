using System;
using System.ComponentModel.DataAnnotations;
namespace rock.Models.UserApi
{
  public class ChangeUserPhoneModel
  {
    [Required, Phone]
    public string Phone { get; set; }
    [Required]
    public byte[] RowVersion { get; set; }
  }
}