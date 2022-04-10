using System;
using System.Collections.Generic;
using rock.Core.Common;

namespace rock.Factories.Adapters
{
  public class MarketStuffIdConfig : IComplexIdConfig<MarketStuffIdShape>
  {
    public int ProductId { get; set; }

    public string Seprator => "-";

    public string Prefix => "MSI";

    public MarketStuffIdShape Shape => new MarketStuffIdShape();

    public void Map(IList<string> i, MarketStuffIdShape o)
    {
      o.ProductId = int.Parse(i[0]);
      o.ProductPriceId = int.Parse(i[1]);
      o.MerchantStuffPriceId = int.Parse(i[2]);
    }

    public void Map(MarketStuffIdShape i, IList<string> o)
    {
      o.Add(i.ProductId.ToString());
      o.Add(i.ProductPriceId.ToString());
      o.Add(i.MerchantStuffPriceId.ToString());
    }
  }

  public class MarketStuffIdShape
  {
    public int ProductId { get; set; }
    public int ProductPriceId { get; set; }
    public int MerchantStuffPriceId { get; set; }
  }
}
