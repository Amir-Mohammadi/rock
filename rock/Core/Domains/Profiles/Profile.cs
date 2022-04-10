using System;
using System.Collections.Generic;
using rock.Core.Domains.Commons;
using rock.Core.Domains.Financial;
using rock.Core.Domains.Users;
using rock.Framework.Models;
namespace rock.Core.Domains.Profiles
{
  public class Profile : IEntity
  {
    public int Id { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public ProfileType Type { get; set; }
    public int? CityId { get; set; }
    public byte[] RowVersion { get; set; }

    public virtual ICollection<ProfileAddress> Addresses { get; set; }
    public virtual PersonProfile PersonProfile { get; set; }
    public virtual FinancialAccount FinancialAccount { get; set; }
    public virtual CompanyProfile CompanyProfile { get; set; }
    public virtual City City { get; set; }

  }
}