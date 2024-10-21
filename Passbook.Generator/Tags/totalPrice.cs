using System.Text.Json;

namespace Passbook.Generator.Tags
{
    public class TotalPrice(string amount, string currencyCode) : SemanticTag("totalPrice")
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
