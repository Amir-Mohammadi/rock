using System;
using System.ComponentModel.DataAnnotations;
using rock.Core.Domains.Users;
namespace rock.Models.UserApi
{
  public class ChangeRoleModel
  {
    [Required]
    public UserRole Roles { get; set; }
    public byte[] RowVersion { get; set; }
  }
}