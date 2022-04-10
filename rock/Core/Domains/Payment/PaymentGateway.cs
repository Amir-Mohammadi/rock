using System;
using rock.Core.Domains.Files;
using rock.Core.Domains.Financial;
using rock.Framework.Models;

namespace rock.Core.Domains.Payment
{
  public class PaymentGateway : IEntity, IRemovable, IHasImage
  {
    public string Gateway { get; set; }
    public DateTime CreatedAt { get; set; }
    public int FinancialAccountId { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid ImageId { get; set; }
    public string ImageTitle { get; set; }
    public string ImageAlt { get; set; }
    public bool IsDefault { get; set; }
    public byte[] RowVersion { get; set; }

    public virtual File Image { get; set; }
    public virtual FinancialAccount FinancialAccount { get; set; }
  }
}