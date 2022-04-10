using System;
using rock.Framework.Models;
namespace rock.Core.Domains.Users
{
  public class UserVerificationCode : IEntity
  {
    public int Id { get; set; }
    public string Context { get; set; }
    public string Code { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public byte[] RowVersion { get; set; }
    public User User { get; set; }
  }
}