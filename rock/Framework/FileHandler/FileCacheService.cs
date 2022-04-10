using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
namespace rock.Framework.FileHandler
{
  public class FileCacheService : IFileCacheService
  {
    private IDistributedCache cache;
    private readonly IServiceScopeFactory serviceScopeFactory;
    public FileCacheService(
      IServiceScopeFactory serviceScopeFactory,
      IDistributedCache memoryCache)
    {
      this.serviceScopeFactory = serviceScopeFactory;
      this.cache = memoryCache;
    }
    public async Task<IFileResult> Get(Guid id, string rowVersion)
    {
      var key = id.ToString();
      IFileResult file;
      byte[] data = cache.Get(key);
      if (data != null)
      {
        file = ByteArrayToObject<IFileResult>(data);
        if (rowVersion == Convert.ToBase64String(file.RowVersion))
        {
          Console.WriteLine("read documentFrom cache " + id.ToString());
          return file;
        }
        else
          cache.Remove(key);
      }
      var token = new CancellationToken(false);
      using (var scope = this.serviceScopeFactory.CreateScope())
      {
        var documentService = scope.ServiceProvider.GetRequiredService<IFileService>();
        file = await documentService.GetFileResultWithStreamById(id: id, cancellationToken: token);
        file.RowVersion = Convert.FromBase64String(rowVersion);
        Console.WriteLine("read documentFrom db " + id.ToString());
      }
      data = ObjectToByteArray(file);
      cache.Set(key, data);
      return file;
    }
    private byte[] ObjectToByteArray<T>(T obj)
    {
      if (obj == null)
        return null;
      BinaryFormatter bf = new BinaryFormatter();
      MemoryStream ms = new MemoryStream();
      bf.Serialize(ms, obj);
      return ms.ToArray();
    }
    private T ByteArrayToObject<T>(byte[] arrBytes)
    {
      MemoryStream memStream = new MemoryStream();
      BinaryFormatter binForm = new BinaryFormatter();
      memStream.Write(arrBytes, 0, arrBytes.Length);
      memStream.Seek(0, SeekOrigin.Begin);
      T obj = (T)binForm.Deserialize(memStream);
      return obj;
    }
  }
}