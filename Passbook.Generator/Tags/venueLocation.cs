using Newtonsoft.Json;

namespace Passbook.Generator.Tags
{
    public class VenueLocation : SemanticTag
    {
        private readonly double _latitude;
        private readonly double _longitude;

        public VenueLocation(double latitude, double longitude) : base("venueLocation")
        {
            _latitude = latitude;
            _longitude = longitude;
        }

        public override void WriteValue(JsonWriter writer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("latitude");
            writer.WriteValue(_latitude);
            writer.WritePropertyName("longitude");
            writer.WriteValue(_longitude);
            writer.WriteEndObject();
        }
    }
}
