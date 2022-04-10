using rock.Core.Domains.Forms;
namespace rock.Core.StaticData
{
  public static class Forms
  {
    private static Form decreaseShopInventory = new()
    {
      Id = 1,
      Title = "کاهش موجودی فروشگاه",
      Description = "کاهش موجودی فروشگاه",
      FormOptions = FormOption.Inventory | FormOption.Decrease | FormOption.Manual
    };
    public static Form DecreaseShopInventory { get => decreaseShopInventory; }
    private static Form increaseShopInventory = new()
    {
      Id = 2,
      Title = "افزایش موجودی انبار",
      Description = "افزایش موجودی انبار",
      FormOptions = FormOption.Inventory | FormOption.Increase | FormOption.Manual
    };
    public static Form IncreaseShopInventory { get => increaseShopInventory; }
    private static Form onlinePaymentFinancialDocument = new()
    {
      Id = 3,
      Title = "سند مالی پرداخت آنلاین",
      Description = "سند مالی پرداخت آنلاین",
      FormOptions = FormOption.Credits | FormOption.Debits | FormOption.Manual
    };
    public static Form OnlinePaymentFinancialDocument { get => onlinePaymentFinancialDocument; }
  }
}