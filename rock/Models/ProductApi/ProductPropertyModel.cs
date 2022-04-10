namespace rock.Models.ProductApi
{
  public class ProductPropertyModel
  {
    public int Id { get; set; }
    public int ProductCategoryPropertyId { get; set; }
    public ProductCategoryPropertyModel ProductCategoryProperty { get; set; }
    public string ExtraKey { get; set; }
    public string Value { get; set; }
    public byte[] RowVersion { get; set; }
  }
}