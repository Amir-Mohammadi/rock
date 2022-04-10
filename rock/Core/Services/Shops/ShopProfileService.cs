using System.Reflection.Metadata.Ecma335;
using System.Threading;
using System.Threading.Tasks;
using rock.Core.Data;
using rock.Core.Domains.Shops;
using rock.Framework.Autofac;

namespace rock.Core.Services.Shops
{
  public class ShopProfileService : IShopProfileService
  {
    private readonly IRepository<ShopProfile> shopProfileRepository;

    public ShopProfileService(IRepository<ShopProfile> shopProfileRepository)
    {
      this.shopProfileRepository = shopProfileRepository;
    }

    public async Task<ShopProfile> CreateShopProfile(ShopProfile shopProfile, Shop shop, CancellationToken cancellationToken)
    {
      shopProfile.ShopId = shop.Id;

      await this.shopProfileRepository.AddAsync(entity: shopProfile,
                                           cancellationToken: cancellationToken);
      return shopProfile;
    }

    public async Task<ShopProfile> UpdateShopProfile(ShopProfile shopProfile, CancellationToken cancellationToken)
    {
      await this.shopProfileRepository.UpdateAsync(entity: shopProfile,
                                           cancellationToken: cancellationToken);
      return shopProfile;
    }

    public async Task<ShopProfile> GetShopProfileById(int shopProfileId, CancellationToken cancellationToken, IInclude<ShopProfile> include = null)
    {
      var shopProfile = await shopProfileRepository.GetAsync(predicate: x => x.Id == shopProfileId,
                                               cancellationToken: cancellationToken,
                                               include: include);
      return shopProfile;
    }

    public async Task<ShopProfile> GetShopProfileByShopId(int shopId, CancellationToken cancellationToken, IInclude<ShopProfile> include = null)
    {
      var shopProfile = await shopProfileRepository.GetAsync(predicate: x => x.ShopId == shopId,
                                               cancellationToken: cancellationToken,
                                               include: include);
      return shopProfile;
    }



  }
}