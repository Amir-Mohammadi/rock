using System;
using System.Collections.Generic;
using System.Linq;
namespace rock.Core.Services.Payment
{
  public class RedirectRequestGenerator
  {
    public string Url { get; set; }
    public string Method { get; set; }
    public Dictionary<string, string> Params { get; set; }
    public RedirectRequestGenerator(string url, string method)
    {
      Url = url;
      Method = method;
      Params = new Dictionary<string, string>();
    }



    const string htmlTemplate = @"
        <form id=""es-payment"" method=""@method"" action=""@action"">
            @params
        </form>
        <script>
          setTimeout(()=>{
            document.getElementById(""es-payment"").submit();
          },500)
         </script>

      ";
    const string paramTemplate = @"<input name=""@key"" type=""hidden"" value=""@value"">";
    public string ToHtmlString()
    {
      var html = htmlTemplate + "";
      var htmlHiddenInputs = "";
      foreach (var key in Params.Keys)
      {
        var value = Params[key];
        var htmlInput = paramTemplate + "";
        htmlInput = htmlInput.Replace("@key", key).Replace("@value", value);
        htmlHiddenInputs += Environment.NewLine;
        htmlHiddenInputs += htmlInput;
      }
      return html.Replace("@action", Url).Replace("@method", Method).Replace("@params", htmlHiddenInputs);
    }
  }
}
