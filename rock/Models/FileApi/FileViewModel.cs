using System;
using rock.Core.Domains.Files;
using rock.Core.Domains.Users;
namespace rock.Models.FileApi
{
  public class FileViewModel
  {
    public Guid Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}