using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace rock.Framework.StateManager
{
  public class StateManagerService : IStateManagerService
  {
    private readonly IDatabase cache;
    public StateManagerService(IDatabase cache)
    {
      this.cache = cache;
    }
    public Task<bool> ClearState()
    {
      throw new NotImplementedException();
    }
    public async Task<T> GetState<T>(string key)
    {
      var value = await cache.StringGetAsync(key);

      if (value.HasValue)
      {
        return await Task.FromResult<T>(JsonSerializer.Deserialize<T>(value));
      }
      else
      {
        return await Task.FromResult<T>(default(T));
      }
    }
    public async Task<bool> RemoveState(string key)
    {
      await cache.KeyDeleteAsync(key);
      return true;
    }
    public async Task<bool> SetState<T>(string key, T value)
    {
      var stringValue = JsonSerializer.Serialize<T>(value);
      await cache.StringSetAsync(key, stringValue);
      return true;
    }
    public async Task<bool> SetState<T>(string key, T value, TimeSpan expiry)
    {
      var stringValue = JsonSerializer.Serialize<T>(value);
      await cache.StringSetAsync(key, stringValue, expiry);
      return true;
    }
  }
}