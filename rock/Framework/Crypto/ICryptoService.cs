using rock.Framework.Autofac;
namespace rock.Framework.Crypto
{
  public interface ICryptoService : IScopedDependency
  {
    string Hash(string values);
    bool Check(string value, string hashedValue);
  }
}