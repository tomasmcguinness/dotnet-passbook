using System.Text.Json;

namespace Passbook.Generator.Tags;

/// <summary>
/// An array of the Apple Music persistent ID for each artist performing at the event, in decreasing order of significance. 
/// Use this key for any type of event ticket.
/// </summary>
public class ArtistIds(params string[] artistIds) : SemanticTag("artistIds")
{

    public override void WriteValue(Utf8JsonWriter writer)
    {
        writer.WriteStartArray();
        foreach (var artistId in artistIds)
        {
            writer.WriteStringValue(artistId);
        }
        writer.WriteEndArray();
    }
}
