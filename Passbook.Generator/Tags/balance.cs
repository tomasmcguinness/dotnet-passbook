using System.Text.Json;

namespace Passbook.Generator.Tags
{
    public class Balance(string amount, string currencyCode) : SemanticTag("balance")
    {
        public override void WriteValue(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("amount");
            writer.WriteStringValue(amount);
            writer.WritePropertyName("currencyCode");
            writer.WriteStringValue(currencyCode);
            writer.WriteEndObject();
        }
    }
}
