using Newtonsoft.Json;

namespace Passbook.Generator.Tags
{
    public class Duration : SemanticTag
    {
        private readonly double _value;

        public Duration(double value) : base("duration")
        {
            _value = value;
        }

        public override void WriteValue(JsonWriter writer)
        {
            writer.WriteValue(_value);
        }
    }
}
