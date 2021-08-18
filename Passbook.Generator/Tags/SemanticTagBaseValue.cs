using Newtonsoft.Json;

namespace Passbook.Generator.Tags
{
    public abstract class SemanticTagBaseValue : SemanticTag
    {
        private readonly object _value;

        public SemanticTagBaseValue(string tag, string value) : base(tag)
        {
            _value = value;
        }

        public SemanticTagBaseValue(string tag, bool value) : base(tag)
        {
            _value = value;
        }

        public SemanticTagBaseValue(string tag, double value) : base(tag)
        {
            _value = value;
        }

        public override void WriteValue(JsonWriter writer)
        {
            writer.WriteValue(_value);
        }
    }
}