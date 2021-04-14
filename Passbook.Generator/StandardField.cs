using Passbook.Generator.Exceptions;

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

        public StandardField(string key, string label, string value, string attributedValue, DataDetectorTypes dataDetectorTypes)
            : this(key, label, value)
        {
            this.AttributedValue = attributedValue;
            this.DataDetectorTypes = dataDetectorTypes;
        }

        public string Value { get; set; }

        protected override void WriteValue(Newtonsoft.Json.JsonWriter writer)
        {
            if (Value == null)
            {
                throw new RequiredFieldValueMissingException(Key);
            }

            writer.WriteValue(Value);
        }

        public override void SetValue(object value)
        {
            this.Value = value as string;
        }

        public override bool HasValue
        {
            get { return !string.IsNullOrEmpty(Value); }
        }
    }
}
