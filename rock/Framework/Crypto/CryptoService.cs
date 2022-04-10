using System;
using System.Security.Cryptography;
using System.Text;

namespace rock.Framework.Crypto
{
  public class CryptoService : ICryptoService
  {
    public bool Check(string value, string hashedValue)
    {
      return Hash(value) == hashedValue;
    }

    public string Hash(string value)
    {
      using (var sha256 = SHA256.Create())
      {
        var byteValue = Encoding.UTF8.GetBytes(value);
        var byteHash = sha256.ComputeHash(byteValue);
        return Convert.ToBase64String(byteHash);
      }
    }
  }
}