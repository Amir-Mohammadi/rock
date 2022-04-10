using System.Linq;
using rock.Framework.Models;
namespace rock.Core.Data
{
  public interface IInclude<TEntity> where TEntity : class, IEntity
  {
    T Execute<T>(T query) where T : IQueryable<TEntity>;
  }
}