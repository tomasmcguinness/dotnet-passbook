using Newtonsoft.Json;
using Passbook.Generator.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Passbook.Generator
{
    public class RelevantLocation
    {
        public double? Altitude { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
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

            if (!string.IsNullOrEmpty(RelevantText))
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
