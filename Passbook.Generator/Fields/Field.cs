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
            this.DataDetectorTypes = new List<DataDetectorType>();
        }

        public Field(string key, string label)
        {
            this.Key = key;
            this.Label = label;
            this.DataDetectorTypes = new List<DataDetectorType>();
        }

        public Field(string key, string label, string changeMessage, FieldTextAlignment textAligment)
        {
            this.Key = key;
            this.Label = label;
            this.ChangeMessage = changeMessage;
            this.TextAlignment = textAligment;
            this.DataDetectorTypes = new List<DataDetectorType>();
        }

        public string Key { get; set; }
        public string Label { get; set; }
        public string ChangeMessage { get; set; }
        public FieldTextAlignment TextAlignment { get; set; }
        public string AttributedValue { get; set; }

        public List<DataDetectorType> DataDetectorTypes { get; private set; }

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

            if (AttributedValue != null)
            {
                writer.WritePropertyName("attributedValue");
                writer.WriteValue(this.AttributedValue);
            }

            writer.WritePropertyName("dataDetectorTypes");
            writer.WriteStartArray();
            DataDetectorTypes.ForEach(t => writer.WriteValue(t.ToSafeString()));
            writer.WriteEndArray();

            WriteKeys(writer);

            writer.WritePropertyName("value");
            WriteValue(writer);

            writer.WriteEndObject();
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
