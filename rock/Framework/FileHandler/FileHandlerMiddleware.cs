using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using rock.Framework.StateManager;
using rock.Models.FileApi;
namespace rock.Framework.FileHandler
{
  public class FileHandlerMiddleware
  {
    private readonly IFileCacheService fileCacheService;
    private readonly IStateManagerService stateManagerService;
    public FileHandlerMiddleware(
      RequestDelegate next,
      IFileCacheService fileCacheService,
      IStateManagerService stateManagerService)
    {
      this.fileCacheService = fileCacheService;
      this.stateManagerService = stateManagerService;
    }
    public async Task Invoke(HttpContext context)
    {
      try
      {
        if (context.Request.Query.Keys.Contains("id"))
        {
          Guid id = Guid.Parse(context.Request.Query["id"]);
          string rowVersion = context.Request.Query["rv"];
          var file = await this.fileCacheService.Get(id, rowVersion);
          context.Response.ContentType = "image/" + file.FileType.Replace(".", "");
          context.Response.Headers[HeaderNames.CacheControl] = Startup.SiteSetting.CacheControl;
          context.Response.Headers[HeaderNames.ETag] = Convert.ToBase64String(file.RowVersion);
          await context.Response.Body.WriteAsync(file.FileStream);
        }
        if (context.Request.Query.Keys.Contains("fK"))
        {
          string fileKey = context.Request.Query["fk"];
          var uploadFileInput = await this.stateManagerService.GetState<UploadFileInput>("fk" + fileKey);
          context.Response.ContentType = "image/" + Path.GetExtension(uploadFileInput.FileName).Replace(".", "");
          context.Response.Headers[HeaderNames.CacheControl] = Startup.SiteSetting.CacheControl;
          context.Response.Headers[HeaderNames.ETag] = fileKey;
          await context.Response.Body.WriteAsync(uploadFileInput.Stream);
        }
      }
      catch (Exception ex)
      {
        context.Response.ContentType = "text/plain";
        await context.Response.WriteAsync(ex.Message);
      }
    }
  }
}