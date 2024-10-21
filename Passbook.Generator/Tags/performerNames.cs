using System.Text.Json;

namespace Passbook.Generator.Tags;

/// <summary>
/// An array of the full names of the performers and opening acts at the event, in decreasing order of significance. Use this key for any type of event ticket.
/// </summary>
public class PerformerNames(params string[] performerNames) : SemanticTag("performerNames")
{
    public override void WriteValue(Utf8JsonWriter writer)
    {
        writer.WriteStartArray();
        foreach (var performerName in performerNames)
        {
            writer.WriteStringValue(performerName);
        }
        writer.WriteEndArray();
    }
}
