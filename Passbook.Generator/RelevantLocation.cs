using Newtonsoft.Json;
using Passbook.Generator.Exceptions;

namespace Passbook.Generator
{
    public class RelevantLocation
    {
        /// <summary>
        /// Optional. Altitude, in meters, of the location.
        /// </summary>
        public double? Altitude { get; set; }

        /// <summary>
        /// Required. Latitude, in degrees, of the location.
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Required. Longitude, in degrees, of the location.
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Optional. Text displayed on the lock screen when the pass is currently relevant.
        /// </summary>
        public string RelevantText { get; set; }

        public void Write(JsonWriter writer)
        {
            Validate();

            writer.WriteStartObject();

            if (Altitude.HasValue)
            {
                writer.WritePropertyName("altitude");
                writer.WriteValue(Altitude.Value);
            }

            writer.WritePropertyName("latitude");
            writer.WriteValue(Latitude);

            writer.WritePropertyName("longitude");
            writer.WriteValue(Longitude);

            if (RelevantText != null)
            {
                writer.WritePropertyName("relevantText");
                writer.WriteValue(RelevantText);
            }

            writer.WriteEndObject();
        }

        private void Validate()
        {
            if (Latitude == double.MinValue)
            {
                throw new RequiredFieldValueMissingException("latitude");
            }

            if (Longitude == double.MinValue)
            {
                throw new RequiredFieldValueMissingException("longitude");
            }
        }
    }
}
