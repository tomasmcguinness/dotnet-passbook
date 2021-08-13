using Newtonsoft.Json;

namespace Passbook.Generator.Tags
{
    public class AirlineCodeSemanticTag : SemanticTag
    {
        private readonly string _airlineCode;

        public AirlineCodeSemanticTag(string airlineCode)
        {
            _airlineCode = airlineCode;
        }

        public override void Write(JsonWriter writer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("airlineCode");
            writer.WriteValue(_airlineCode);

            writer.WriteEndObject();
        }
    }
}
