using Newtonsoft.Json;

namespace Passbook.Generator.Tags
{
    public class StringSemanticTag
    {
        private readonly string _value;
        private readonly string _tag;

        public StringSemanticTag(string tag, string value)
        {
            _tag = tag;
            _value = value;
        }

        public void Write(JsonWriter writer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName(_tag);
            writer.WriteValue(_value);

            writer.WriteEndObject();
        }

    }
}