namespace rock.Framework.Models

{
  public interface IHasRowVersion
  {
    byte[] RowVersion { get; set; }
  }
}