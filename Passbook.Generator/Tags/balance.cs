using Newtonsoft.Json;

namespace Passbook.Generator.Tags
{
    public class Balance : SemanticTag
    {
        private readonly string _amount;
        private readonly string _currencyCode;

        public Balance(string amount, string currencyCode) : base("balance")
        {
            _amount = amount;
            _currencyCode = currencyCode;
        }

        public override void WriteValue(JsonWriter writer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("amount");
            writer.WriteValue(_amount);
            writer.WritePropertyName("currencyCode");
            writer.WriteValue(_currencyCode);
            writer.WriteEndObject();
        }
    }
}
