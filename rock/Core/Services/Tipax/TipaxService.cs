using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using rock.Core.Domains.Orders;
using rock.Core.Domains.Profiles;
using rock.Core.Extensions;
using rock.Core.Services.Tipax.TipaxEnums;
using rock.Core.Services.Tipax.TipaxModel;
using rock.Core.Services.Tipax.TipaxResponse;
using rock.Framework.Setting.TransportSetting;
using rock.Framework.StateManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace rock.Core.Services.Tipax
{
  public class TipaxService : ITipaxService
  {
    private const string Key = "TIPAX";
    private readonly IOptionsSnapshot<TipaxInfo> tipaxInfo;
    private readonly IStateManagerService stateManagerService;
    private HttpClient client;
    private string token = string.Empty;
    public TipaxService(IOptionsSnapshot<TipaxInfo> tipaxInfo,
                        IStateManagerService stateManagerService)
    {
      this.tipaxInfo = tipaxInfo;
      this.stateManagerService = stateManagerService;
      client = new HttpClient();
    }
    private async Task<string> GetAuthriztionToken()
    {

      if (string.IsNullOrEmpty(token) || string.IsNullOrWhiteSpace(token))
      {
        string url = tipaxInfo.Value.TipaxApiInfo.Login;
        var body = new TipaxBaseModel<LoginModel>
        {

          SystemToken = tipaxInfo.Value.TipaxLoginInfo.SystemToken,
          UserToken = null,
          Item = new LoginModel
          {
            UserName = tipaxInfo.Value.TipaxLoginInfo.Username,
            Password = tipaxInfo.Value.TipaxLoginInfo.Password
          }
        };

        var response = await client.PostAsync(url, body.GetStringContent(Encoding.UTF8));

        string responseBody = await response.Content.ReadAsStringAsync();

        var loginResult = JsonConvert.DeserializeObject<TipaxItemResponse<TipaxLoginResult>>(responseBody);

        token = loginResult.Item.Token;
      }
      return token;

    }

    private async Task<TipaxCityResult> GetCityByName(string cityName)
    {
      var cities = await LoadCities();
      return cities.FirstOrDefault(x => x.Name.Trim() == cityName.Trim() && x.ParentID != 0);
    }

    private async Task<List<TipaxCityResult>> LoadCities()
    {
      var cities = await stateManagerService.GetState<List<TipaxCityResult>>(generateKey(Key, "Cities"));
      if (cities == null)
      {
        var token = await GetAuthriztionToken();
        string url = tipaxInfo.Value.TipaxApiInfo.Cities;
        var body = new TipaxBaseModel
        {
          SystemToken = tipaxInfo.Value.TipaxLoginInfo.SystemToken,
          UserToken = token,
        };

        var response = await client.PostAsync(url, body.GetStringContent(Encoding.UTF8));

        string responseBody = await response.Content.ReadAsStringAsync();

        var result = JsonConvert.DeserializeObject<TipaxListItemResponse<TipaxCityResult>>(responseBody);

        cities = result.Rows.ToList();
        await stateManagerService.SetState<List<TipaxCityResult>>(generateKey(Key, "Cities"), cities);

      }
      return cities;

    }

    private async Task<List<PackagingTypeResult>> GetPackagingType()
    {
      string jsonPackagingTypes = await stateManagerService.GetState<string>(key: generateKey(Key, "PackagingTypes"));
      var packagingTypes = new List<PackagingTypeResult>();
      if (string.IsNullOrEmpty(jsonPackagingTypes) || string.IsNullOrWhiteSpace(jsonPackagingTypes))
      {
        var token = await GetAuthriztionToken();
        string url = tipaxInfo.Value.TipaxApiInfo.PackagingType;
        var body = new TipaxBaseModel
        {
          SystemToken = tipaxInfo.Value.TipaxLoginInfo.SystemToken,
          UserToken = token,
        };

        var response = await client.PostAsync(url, body.GetStringContent(Encoding.UTF8));

        string responseBody = await response.Content.ReadAsStringAsync();

        var result = JsonConvert.DeserializeObject<TipaxListItemResponse<PackagingTypeResult>>(responseBody);

        packagingTypes = result.Rows.ToList();
        await stateManagerService.SetState<List<PackagingTypeResult>>(generateKey(Key, "PackagingTypes"), packagingTypes);

      }

      packagingTypes = JsonConvert.DeserializeObject<List<PackagingTypeResult>>(jsonPackagingTypes);

      return packagingTypes;
    }

    public async Task<int> CalculateContractPrice(List<CartItem> cartItems, ProfileAddress profileAddress, CancellationToken cancellationToken)
    {

      #region Get Token
      var token = await GetAuthriztionToken();
      #endregion

      #region Get Reciever City
      var recieverCity = await GetCityByName(cityName: profileAddress.City.Name);
      #endregion

      #region  Get Sender City
      var senderCityName = cartItems.FirstOrDefault().ProductPrice.City.Name;
      var senderCity = await GetCityByName(cityName: senderCityName);
      #endregion

      #region Calculate Price
      var price = cartItems.Sum(x => x.ProductPrice.Price);
      #endregion

      #region Prepare Dispathes
      var dispathes = cartItems.Select(m => new CalculateContractDispatchModel
      {
        GoodKindID = (int)GoodKind.Other,
        Weight = m.Product.ProductShippingInfo.Weight,
        Height = m.Product.ProductShippingInfo.Height,
        Width = m.Product.ProductShippingInfo.Width,
        Length = m.Product.ProductShippingInfo.Length,
      });
      #endregion

      #region Prepare Model 
      var calculateModel = new TipaxBaseModel<TipaxCalculateContractModel>
      {
        SystemToken = tipaxInfo.Value.TipaxLoginInfo.SystemToken,
        UserToken = token,
        Item = new TipaxCalculateContractModel
        {
          Price = price,
          SenderCityID = senderCity.ID,
          ReceiverCityID = recieverCity.ID,
          Dispatchs = dispathes.ToArray()
        }

      };
      #endregion

      #region  Api Call And Get Result
      string url = tipaxInfo.Value.TipaxApiInfo.CalculateContract;
      var body = calculateModel;

      var response = await client.PostAsync(url, body.GetStringContent(Encoding.UTF8));

      string responseBody = await response.Content.ReadAsStringAsync();

      var result = JsonConvert.DeserializeObject<TipaxItemResponse<TipaxCalculateContractResult>>(responseBody);

      return (int)result.Item.FinalAmount / 10;
      #endregion
    }
    private string generateKey(string prefix, string postfix)
    {
      return prefix + "_" + postfix;
    }
  }
}
