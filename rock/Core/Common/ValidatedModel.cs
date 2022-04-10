using System;
using rock.Core.Common.Exception;

namespace rock.Core.Common
{
  public class ValidatedModel : IValidatedModel
  {
    private int? errorCode = 0;
    private object additionalData;

    public ValidatedModel(int? errorCode = null, object additionalData = null)
    {
      this.errorCode = errorCode;
      this.additionalData = additionalData;
    }

    public int? GetErrorCode()
    {
      return errorCode;
    }

    public bool IsNotOk()
    {
      return !IsOk();
    }

    public bool IsOk()
    {
      if (errorCode.HasValue)
      {
        return false;
      }
      return true;
    }

    public void PassOrDie()
    {
      if (IsOk()) return;
      throw new FailedException(GetErrorCode().Value, additionalData);
    }

  }
}
