using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using rock.Core.Extensions;
using rock.Framework.Setting;

namespace rock.Core.Services.Common
{
  public class TelephonyService : ITelephonyService
  {

    private readonly SMSServiceInfo smsServiceInfo;
    private HttpClient client;
    public TelephonyService(IOptionsSnapshot<SiteSettings> settings)
    {
      this.smsServiceInfo = settings.Value.SMSServiceInfo;
      this.client = new HttpClient();
    }
    public Task SendSMS(string phone, string message, CancellationToken cancellationToken)
    {
      return Task.Run(() =>
      {
        string resultMessage = message;
        string url = string.Format(smsServiceInfo.LookUpUrl, smsServiceInfo.APIKEY, phone, resultMessage);
        var result = client.GetAsync(url).Result;
      });
    }



  }
}
