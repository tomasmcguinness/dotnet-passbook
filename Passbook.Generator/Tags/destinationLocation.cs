using System.Text.Json;

namespace Passbook.Generator.Tags;

public class DestinationLocation(double latitude, double longitude) : SemanticTag("destinationLocation ")
{    public override void WriteValue(Utf8JsonWriter writer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("latitude");
        writer.WriteNumberValue(latitude);
        writer.WritePropertyName("longitude");
        writer.WriteNumberValue(longitude);
        writer.WriteEndObject();
    }
}
