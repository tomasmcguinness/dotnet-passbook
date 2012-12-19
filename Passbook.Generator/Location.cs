using Newtonsoft.Json;
using Passbook.Generator.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Passbook.Generator
{
    public class Location
    {
        public Location()
        {
            Latitude = double.MinValue;
            Longitude = double.MinValue;
        }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public void Write(JsonWriter writer)
        {
            Validate();

            writer.WriteStartObject();
            writer.WritePropertyName("latitude");
            writer.WriteValue(Latitude);
            writer.WritePropertyName("longitude");
            writer.WriteValue(Longitude);
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
