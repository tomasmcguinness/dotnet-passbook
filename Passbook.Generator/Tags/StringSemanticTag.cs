using Newtonsoft.Json;

namespace Passbook.Generator.Tags
{
    public class StringSemanticTag : SemanticTag
    {
        private readonly string _value;

        public StringSemanticTag(string tag, string value) : base(tag)
        {
            _value = value;
        }

        public override void WriteValue(JsonWriter writer)
        {
            writer.WriteValue(_value);
        }
    }
}