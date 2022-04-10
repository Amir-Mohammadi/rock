using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rock.Core.Services.Tipax.TipaxResponse
{
    public class TipaxLoginResult
    {
        public string Token { get; set; }
        public int Expire { get; set; }
    }
}
