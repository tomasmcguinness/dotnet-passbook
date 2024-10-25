using System.Text.Json;

namespace Passbook.Generator.Fields
{
    public class NumberField : Field
    {
        public NumberField()
            : base()
        { }

        public NumberField(string key, string label, decimal value, FieldNumberStyle numberStyle)
            : base(key, label)
        {
            Value = value;
            NumberStyle = numberStyle;
        }

        public NumberField(string key, string label, int value, FieldNumberStyle numberStyle)
            : this(key, label, (decimal)value, numberStyle)
        {
        }

        /// <summary>
        /// ISO 4217 currency code for the field’s value.
        /// </summary>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Style of number to display. Must be one of <see cref="FieldNumberStyle" />
        /// </summary>
        public FieldNumberStyle NumberStyle { get; set; }

        public decimal Value { get; set; }

        protected override void WriteKeys(Utf8JsonWriter writer)
        {
            if (!string.IsNullOrEmpty(CurrencyCode))
            {
                writer.WritePropertyName("currencyCode");
                writer.WriteStringValue(CurrencyCode);
            }

            if (NumberStyle != FieldNumberStyle.Unspecified)
            {
                writer.WritePropertyName("numberStyle");
                writer.WriteStringValue(NumberStyle.ToString());
            }
        }

        protected override void WriteValue(Utf8JsonWriter writer)
        {
            writer.WriteNumberValue(Value);
        }

        public override void SetValue(object value)
        {
            Value = (decimal)value;
        }

        public override bool HasValue
        {
            get { return true; }
        }
    }
}
