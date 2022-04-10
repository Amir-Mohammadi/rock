namespace rock.Models.CustomerApi
{
  public class ShoppingProductModel
  {
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public ShoppingProductBrandModel ProductBrand { get; set; }
    public ShoppingProductColorModel ProductColor { get; set; }
    public ShoppingProductPreviewImageModel ProductPreviewImage { get; set; }
    public ShoppingProductPriceModel ProductPrice { get; set; }
    
    public byte[] RowVersion { get; set; }
  }
}