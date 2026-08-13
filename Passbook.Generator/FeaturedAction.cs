using System.Text.Json;

namespace Passbook.Generator;

/// <summary>
/// A quick-action link shown below a pass, such as opening a related collection or a membership benefits page.
/// </summary>
public class FeaturedAction
{
    /// <summary>
    /// A unique identifier for the featured action.
    /// </summary>
    public string Identifier { get; set; }

    /// <summary>
    /// The type of the featured action, such as "place" or "membershipBenefits".
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// The URL the system opens when someone selects the featured action.
    /// </summary>
    public string Url { get; set; }

    public void Write(Utf8JsonWriter writer)
    {
        writer.WriteStartObject();

        writer.WritePropertyName("identifier");
        writer.WriteStringValue(Identifier);

        writer.WritePropertyName("type");
        writer.WriteStringValue(Type);

        writer.WritePropertyName("url");
        writer.WriteStringValue(Url);

        writer.WriteEndObject();
    }
}
