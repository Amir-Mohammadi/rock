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
  public class ShopModel
  {
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public int OwnerId { get; set; }
    [Required]
    public int CityId { get; set; }
    public bool Active { get; set; }
    public string PostalCode { get; set; }
    public string Address { get; set; }
    public string Telephone { get; set; }
    public string Website { get; set; }
    public CityModel City { get; set; }
    public UserModel Owner { get; set; }
    public DateTime CreatedAt { get; set; }
    public byte[] RowVersion { get; set; }
  }
}