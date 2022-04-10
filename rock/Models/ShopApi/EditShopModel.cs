using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using rock.Core.Domains.Profiles;
using rock.Models.CommonApi;
using rock.Models.UserApi;

namespace rock.Models.ShopApi
{
  public class EditShopModel
  {
    public string Name { get; set; }
    public string Website { get; set; }
    public string Address { get; set; }
    public string Telephone { get; set; }
    [Required]
    public byte[] RowVersion { get; set; }
  }
}