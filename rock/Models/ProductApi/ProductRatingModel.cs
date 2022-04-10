using System;
using System.Collections.Generic;
using rock.Models.UserApi;

namespace rock.Models.ProductApi
{
    public class ProductRatingModel
    {
    public int Id { get; set; }
    public string Title { get; set; }

    public IList<string> Conditions { get; set; }

    public SimpleUserModel User { get; set; }

    public byte[] RowVersion { get; set; }
    }
}
