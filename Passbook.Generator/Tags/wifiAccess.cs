using Newtonsoft.Json;

namespace Passbook.Generator.Tags
{
    public class WifiAccess : SemanticTag
    {
        private readonly string _ssid;
        private readonly string _password;

        public WifiAccess(string ssid, string password) : base("wifiAccess")
        {
            _ssid = ssid;
            _password = password;
        }

        public override void WriteValue(JsonWriter writer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("password");
            writer.WriteValue(_ssid);
            writer.WritePropertyName("ssid");
            writer.WriteValue(_password);
            writer.WriteEndObject();
        }
    }
}
