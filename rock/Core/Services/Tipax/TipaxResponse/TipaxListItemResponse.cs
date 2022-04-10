using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rock.Core.Services.Tipax.TipaxResponse
{
    public class TipaxListItemResponse<T>
    {
        public T[] Rows { get; set; }
    }
}
