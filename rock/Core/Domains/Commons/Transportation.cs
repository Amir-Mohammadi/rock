using System;
using System.Collections.Generic;
using rock.Core.Domains.Orders;
using rock.Framework.Models;
namespace rock.Core.Domains.Commons
{
  public class Transportation : IEntity, IRemovable, IHasDescription
  {
    public int Id { get; set; }
    public int FromCityId { get; set; }
    public int ToCityId { get; set; }
    public int Distance { get; set; }
    public int Cost { get; set; }
    public DateTime? DeletedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }

    public virtual City FromCity { get; set; }
    public virtual City ToCity { get; set; }

  }
}