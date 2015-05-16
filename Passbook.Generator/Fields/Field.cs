using System;
using Newtonsoft.Json;
using Passbook.Generator.Exceptions;
using System.Collections.Generic;
using System.Text;

namespace Passbook.Generator.Fields
{
    public abstract class Field
    {
        public Field()
        {
			this.DataDetectorTypes = DataDetectorTypes.PKDataDetectorAll;
        }

		public Field(string key, string label)
			: this()
        {
            this.Key = key;
            this.Label = label;
        }

		public Field(string key, string label, string changeMessage, FieldTextAlignment textAligment)
			: this(key, label)
        {
            this.ChangeMessage = changeMessage;
            this.TextAlignment = textAligment;
        }

        public string Key { get; set; }
        public string Label { get; set; }
        public string ChangeMessage { get; set; }
        public FieldTextAlignment TextAlignment { get; set; }
        public string AttributedValue { get; set; }

        public DataDetectorTypes DataDetectorTypes { get; protected set; }

        public void Write(JsonWriter writer)
        {
            Validate();

            writer.WriteStartObject();

            writer.WritePropertyName("key");
            writer.WriteValue(Key);

            if (!string.IsNullOrEmpty(ChangeMessage))
            {
                writer.WritePropertyName("changeMessage");
                writer.WriteValue(ChangeMessage);
            }

            if (!string.IsNullOrEmpty(Label))
            {
                writer.WritePropertyName("label");
                writer.WriteValue(Label);
            }

            if (TextAlignment != FieldTextAlignment.Unspecified)
            {
                writer.WritePropertyName("textAlignment");
                writer.WriteValue(TextAlignment.ToString());
            }

			if (!string.IsNullOrEmpty(AttributedValue))
            {
                writer.WritePropertyName("attributedValue");
                writer.WriteValue(this.AttributedValue);
            }

            WriteKeys(writer);

            WriteDataDetectorTypes(writer);

            writer.WritePropertyName("value");
            WriteValue(writer);

            writer.WriteEndObject();
        }

        private void WriteDataDetectorTypes(JsonWriter writer)
        {
			if (DataDetectorTypes != Fields.DataDetectorTypes.PKDataDetectorAll)
			{
				writer.WritePropertyName("dataDetectorTypes");
				writer.WriteStartArray();

				foreach (Enum value in Enum.GetValues(typeof(DataDetectorTypes)))
					if (value.CompareTo(DataDetectorTypes.PKDataDetectorNone) != 0 && DataDetectorTypes.HasFlag(value))
						writer.WriteValue(value.ToString());

				writer.WriteEndArray();
			}
        }

        private void Validate()
        {
            if (string.IsNullOrEmpty(Key))
            {
                throw new RequiredFieldValueMissingException("key");
            }
        }

        protected virtual void WriteKeys(JsonWriter writer)
        {
            // NO OP
        }

        protected abstract void WriteValue(JsonWriter writer);

        public abstract void SetValue(object value);

        public abstract bool HasValue { get; }
    }
}
