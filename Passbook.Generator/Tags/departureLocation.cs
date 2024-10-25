using System.Text.Json;

namespace Passbook.Generator.Tags
{
    public class DepartureLocation(double latitude, double longitude) : SemanticTag("departureLocation")
    {
        public override void WriteValue(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("latitude");
            writer.WriteNumberValue(latitude);
            writer.WritePropertyName("longitude");
            writer.WriteNumberValue(longitude);
            writer.WriteEndObject();
        }
    }
}
