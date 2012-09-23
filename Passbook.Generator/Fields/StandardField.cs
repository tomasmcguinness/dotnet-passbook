using Passbook.Generator.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passbook.Generator.Fields
{
    public class StandardField : Field
    {
        public StandardField()
            : base()
        { }

        public StandardField(string key, string label, string value)
            : base(key, label)
        {
            this.Value = value;
        }

        public string Value { get; set; }

        protected override void WriteValue(Newtonsoft.Json.JsonWriter writer)
        {
            if (Value == null)
            {
                throw new RequiredFieldValueMissingException("value");
            }

            writer.WriteValue(Value);
        }
    }
}
