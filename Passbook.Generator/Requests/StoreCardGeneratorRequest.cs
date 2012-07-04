using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passbook.Generator
{
    public class StoreCardGeneratorRequest : PassGeneratorRequest
    {
        public double Balance { get; set; }
        public object OwnersName { get; set; }
    }
}
