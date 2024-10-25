using System.Text.Json;

namespace Passbook.Generator.Tags;

public class WifiAccess(string ssid, string password) : SemanticTag("wifiAccess")
{
    public override void WriteValue(Utf8JsonWriter writer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("ssid");
        writer.WriteStringValue(ssid);
        writer.WritePropertyName("password");
        writer.WriteStringValue(password);
        writer.WriteEndObject();
    }
}
