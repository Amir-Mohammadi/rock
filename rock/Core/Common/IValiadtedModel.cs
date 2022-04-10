namespace rock.Core.Common
{
  public interface IValidatedModel
  {
    bool IsOk();
    bool IsNotOk();
    void PassOrDie();
    int? GetErrorCode();
  }
}