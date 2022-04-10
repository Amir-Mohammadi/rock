using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using rock.Core.Domains.Commons;
using rock.Framework.Models;

namespace rock.Models.CommonApi
{
  public class StaticsModel
  {
    public int Id { get; set; }

    [Required]
    public string Key { get; set; }

    [Required]
    public string Value { get; set; }

    public StaticType StaticType { get; set; }

    public byte[] RowVersion { get; set; }
  }
}