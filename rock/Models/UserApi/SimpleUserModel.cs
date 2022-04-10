using System;
using rock.Core.Domains.Users;

namespace rock.Models.UserApi
{
  public class SimpleUserModel
  {
    public int Id { get; set; }
    public int ProfileId { get; set; }
    public string FullName { get; set; }
    public UserRole Role { get; set; }
  }
}
