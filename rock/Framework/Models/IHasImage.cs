using System;
using rock.Core.Domains.Files;
namespace rock.Framework.Models
{
  public interface IHasImage : IHasRowVersion
  {
    Guid ImageId { get; set; }
    string ImageTitle { get; set; }
    string ImageAlt { get; set; }
    File Image { get; set; }
  }
}