using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passbook.Generator
{
    public abstract class Field
    {
        public string Key { get; set; }
        public string Label { get; set; }
        public string ChangeMessage { get; set; }
        public FieldTextAlignment TextAlignment { get; set; }
    }
}
