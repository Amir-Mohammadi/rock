using rock.Core.Domains.Threads;
using rock.Models.UserApi;

namespace rock.Models.CommonApi
{
  public class ThreadActivityModel
  {
    public int Id { get; set; }
    public ThreadActivityType Type { get; set; }
    public string Payload { get; set; }
    public int? ReferenceId { get; set; }
    public int UserId { get; set; }
    public byte[] RowVersion { get; set; }
    public ThreadActivityModel Reference { get; set; }
    public UserModel User { get; set; }
  }
}