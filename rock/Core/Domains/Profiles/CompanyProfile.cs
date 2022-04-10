using System;
using rock.Framework.Models;
namespace rock.Core.Domains.Profiles
{
  public class CompanyProfile : IEntity
  {
    public int Id { get; set; }
    public string RegistrationCode { get; set; }
    public string Name { get; set; }
    public int ProfileId { get; set; }
    public virtual Profile Profile { get; set; }
    public byte[] RowVersion { get; set; }
  }
}