using System.Text.Json;

namespace Passbook.Generator.Tags;

/// <summary>
/// The type of event. Use this key for any type of event ticket.
/// </summary>
public class EventType(EventTypes eventType) : SemanticTag("eventType")
{
    public override void WriteValue(Utf8JsonWriter writer)
    {
        writer.WriteStringValue(eventType.ToString());
    }
}
