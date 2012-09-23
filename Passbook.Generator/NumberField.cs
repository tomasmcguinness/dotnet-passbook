using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passbook.Generator
{
    public class NumberField : Field
    {
        public string Currency { get; set; }
        public NumberStyle Style { get; set; }
        public long Value { get; set; }
    }
}
