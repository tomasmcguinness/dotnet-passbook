using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passbook.Generator.Fields
{
    public class NumberField : Field
    {
        public NumberField(string key, string label, int value, FieldNumberStyle numberStyle)
            : base(key, label)
        {
            this.Value = value;
            this.NumberStyle = numberStyle;
        }
        public string CurrencyCode { get; set; }
        public FieldNumberStyle NumberStyle { get; set; }
        public long Value { get; set; }

        protected override void WriteKeys(Newtonsoft.Json.JsonWriter writer)
        {
            if (CurrencyCode != null)
            {
                writer.WritePropertyName("currencyCode");
                writer.WriteValue(CurrencyCode);
            }

            if (NumberStyle != FieldNumberStyle.Unspecified)
            {
                writer.WritePropertyName("numberStyle");
                writer.WriteValue(NumberStyle.ToString());
            }
        }

        protected override void WriteValue(Newtonsoft.Json.JsonWriter writer)
        {
            writer.WriteValue(Value);
        }
    }
}
