using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using rock.Core.Common;
using rock.Core.Domains.Commons;
using rock.Core.Domains.Financial;
using rock.Core.Domains.Forms;
using rock.Core.Domains.Products;
using rock.Core.Domains.Users;
using rock.Core.Extensions;
using rock.Models.CommonApi;
using rock.Models.FinancialApi;
using rock.Models.ProductApi;
using rock.Models.UserApi;

namespace rock.Factories
{
  public class BaseFactory
  {
    protected IList<TResult> CreateModelList<TInput, TResult>(IEnumerable<TInput> source,
                                                              Func<TInput, TResult> convertFunction)
    {
      return source.ToList().Convert(convertFunction).ToList();
    }
    protected async Task<IList<TResult>> CreateModelListAsync<TInput, TResult>(IEnumerable<TInput> source,
                                                                               Func<TInput, TResult> convertFunction,
                                                                               CancellationToken cancellationToken)
    {
      var list = await source.AsQueryable().ToListAsync(cancellationToken);
      var result = list.Convert(convertFunction).ToList();
      return result;
    }
    protected IPagedList<TResult> CreateModelPagedList<TInput, TResult>(IEnumerable<TInput> source,
                                                                        Func<TInput, TResult> convertFunction,
                                                                        string sortBy,
                                                                        string order,
                                                                        int pageIndex,
                                                                        int pageSize)
    {
      source = source.Sort(order: order, sortBy: sortBy);
      var result = PagedList<TResult>.CreatePagedList(source: source.AsQueryable(),
                                                      pageIndex: pageIndex,
                                                      pageSize: pageSize,
                                                      convertFunction: convertFunction);
      return result;
    }
    protected async Task<IPagedList<TResult>> CreateModelPagedListAsync<TInput, TResult>(IEnumerable<TInput> source,
      Func<TInput, TResult> convertFunction,
      string sortBy,
      string order,
      int pageIndex,
      int pageSize,
      CancellationToken cancellationToken)
    {
      source = source.Sort(order: order, sortBy: sortBy);
      var result = await PagedList<TResult>.CreatePagedListAsync(source: source.AsQueryable(),
                                                                 pageIndex: pageIndex,
                                                                 pageSize: pageSize,
                                                                 convertFunction: convertFunction,
                                                                 cancellationToken: cancellationToken);
      return result;
    }
    protected CityModel createCityModel(City city)
    {
      if (city == null)
        return null;
      return new CityModel
      {
        Id = city.Id,
        Name = city.Name,
        ProvinceId = city.ProvinceId,
        Province = this.CreateProvinceModel(province: city.Province),
        RowVersion = city.RowVersion,
      };
    }
    protected ProductImageModel createProductImageModel(ProductImage productImage)
    {
      if (productImage == null)
        return null;
      return new ProductImageModel()
      {
        Id = productImage.Id,
        ImageId = productImage.ImageId,
        Order = productImage.Order,
        ImageAlt = productImage.ImageAlt,
        ImageTitle = productImage.ImageTitle,
        RowVersion = productImage.RowVersion
      };
    }
    protected ProvinceModel CreateProvinceModel(Province province)
    {
      if (province == null)
        return null;
      return new ProvinceModel()
      {
        Id = province.Id,
        Name = province.Name,
        AreaCode = province.AreaCode,
        RowVersion = province.RowVersion,
      };
    }

    protected UserModel createUserModel(User user)
    {
      if (user == null)
        return null;
      return new UserModel()
      {
        Birthday = user.Profile?.PersonProfile?.Birthdate,
        City = this.createCityModel(user.Profile?.City),
        EconomicCode = user.Profile?.PersonProfile?.EconomicCode,
        Email = user.Profile?.Email,
        Enabled = user.Enabled,
        FatherName = user.Profile?.PersonProfile?.FatherName,
        FirstName = user.Profile?.PersonProfile?.FirstName,
        Gender = user.Profile?.PersonProfile?.Gender,
        Id = user.Id,
        LastName = user.Profile?.PersonProfile?.LastName,
        NationalCode = user.Profile?.PersonProfile?.NationalCode,
        Phone = user.Profile?.Phone,
        PictureId = user.Profile?.PersonProfile?.PictureId,
        Roles = user.Role,
        cityId = user.Profile.CityId,
        RowVersion = user.Profile?.RowVersion
      };
    }
    public CurrencyModel CreateCurrencyModel(Currency currency)
    {
      if (currency == null)
        return null;
      return new CurrencyModel()
      {
        Id = currency.Id,
        Name = currency.Name,
        Ratio = currency.Ratio,
        Symbol = currency.Symbol,
        RowVersion = currency.RowVersion
      };
    }
    public BankModel CreateBankModel(Bank bank)
    {
      if (bank == null) return null;
      return new BankModel()
      {
        Id = bank.Id,
        Name = bank.Name,
        RowVersion = bank.RowVersion
      };
    }
    public FinancialFormModel CreateFinancialFormModel(Form form)
    {
      if (form == null) return null;

      return new FinancialFormModel()
      {
        Description = form.Description,
        Id = form.Id,
        RowVersion = form.RowVersion,
        Title = form.Title
      };
    }
  }
}