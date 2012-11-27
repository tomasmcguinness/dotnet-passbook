using System;
using Newtonsoft.Json;
using Passbook.Generator.Exceptions;

namespace Passbook.Generator.Fields
{
    public abstract class Field
    {
        public Field()
        { }

        public Field(string key, string label)
        {
            this.Key = key;
            this.Label = label;
        }

        public Field(string key, string label, string changeMessage, FieldTextAlignment textAligment)
        {
            this.Key = key;
            this.Label = label;
            this.ChangeMessage = changeMessage;
            this.TextAlignment = textAligment;
        }

        public string Key { get; set; }
        public string Label { get; set; }
        public string ChangeMessage { get; set; }
        public FieldTextAlignment TextAlignment { get; set; }

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
    }
}
