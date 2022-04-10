namespace rock.Models
{
  public class Key<T>
  {
    public Key(T id)
    {
      this.Id = id;
    }
    public T Id { get; }
  }
}