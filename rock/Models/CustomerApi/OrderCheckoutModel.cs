using System.Collections.Generic;

namespace rock.Models.CustomerApi
{
  public class OrderCheckoutModel
  {
    public IList<OrderCheckoutBankBillModel> BankBills { get; set; }
    public OrderCheckoutFactureModel Facture { get; set; }
    public IList<OrderCheckoutCargoModel> Cargos { get; set; }
  }
}