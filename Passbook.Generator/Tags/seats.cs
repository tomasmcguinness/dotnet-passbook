using System.Text.Json;

namespace Passbook.Generator.Tags;

public class Seats(params Seat[] seats) : SemanticTag("seats")
{
    public override void WriteValue(Utf8JsonWriter writer)
    {
        writer.WriteStartArray();
        
        foreach(var seat in seats)
        {
            writer.WriteStartObject();

            writer.WriteEndObject();
        }

        writer.WriteEndArray();
    }
}
