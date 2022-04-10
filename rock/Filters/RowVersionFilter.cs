using System;
using Microsoft.AspNetCore.Mvc;

namespace rock.Filters
{
  public class RowVersionFilter<IdType>
  {
    [FromRoute(Name = "id")]
    public IdType Id { get; set; }

    [FromRoute(Name = "rowVersion")]
    public string Base64RowVersion { get; set; }

    public byte[] GetRowVersion()
    {
      return Microsoft.AspNetCore.WebUtilities.Base64UrlTextEncoder.Decode(Base64RowVersion);
    }
  }
}
