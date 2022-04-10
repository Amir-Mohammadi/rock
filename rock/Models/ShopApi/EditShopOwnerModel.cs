using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using rock.Core.Domains.Profiles;
using rock.Models.CommonApi;
using rock.Models.UserApi;

namespace rock.Models.ShopApi
{
  public class EditShopOwnerModel
  {
    [Required]
    public int OwnerId { get; set; }
  }
}