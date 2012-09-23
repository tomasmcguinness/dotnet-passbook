using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passbook.Generator
{
    public class DateField : StandardField
    {
        public DateTime Value { get; set; }
        public FieldDateTimeStyle DateStyle { get; set; }
        public FieldDateTimeStyle TimeStyle { get; set; }
        public bool IsRelavent { get; set; }
    }
}
