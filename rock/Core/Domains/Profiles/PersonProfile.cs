using System;
using rock.Core.Domains.Files;
using rock.Framework.Models;
namespace rock.Core.Domains.Profiles
{
  public class PersonProfile : IEntity
  {
    public int Id { get; set; }
    public string NationalCode { get; set; }
    public string EconomicCode { get; set; }
    public DateTime? Birthdate { get; set; }
    public string FatherName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Gender? Gender { get; set; }
    public Guid? PictureId { get; set; }
    public int ProfileId { get; set; }
    public virtual File Picture { get; set; }
    public virtual Profile Profile { get; set; }
    public byte[] RowVersion { get; set; }
  }
}