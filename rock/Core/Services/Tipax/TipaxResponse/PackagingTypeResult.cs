using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rock.Core.Services.Tipax.TipaxResponse
{
    public class PackagingTypeResult
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool IsPakat { get; set; }
    }
}
