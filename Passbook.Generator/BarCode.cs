using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Passbook.Generator
{
    public class BarCode
    {
        public BarcodeType Type { get; set; }
        public string Message { get; set; }
        public string Encoding { get; set; }
        public string AlternateText { get; set; }
    }
}
