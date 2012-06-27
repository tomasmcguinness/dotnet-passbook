using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Passbook.Generator
{
    public class PassGeneratorRequest
    {
        [JsonProperty("passTypeIdentifier")]
        public string Identifier { get; set; }
        [JsonProperty("formatVersion")]
        public int FormatVersion { get; set; }
        [JsonProperty("serialNumber")]
        public string SerialNumber { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("teamIdentifier")]
        public string TeamIdentifier { get; set; }
        [JsonProperty("organizationName")]
        public string OrganizationName { get; set; }

        [JsonIgnore]
        public string IconFile { get; set; }
        [JsonIgnore]
        public string IconRetinaFile { get; set; }

        [JsonIgnore]
        public string LogoFile { get; set; }
        [JsonIgnore]
        public string LogoRetinaFile { get; set; }

        [JsonProperty("eventTicket")]
        public EventTicket Event { get; set; }
        [JsonProperty("backgroundColor")]
        public string BackgroundColor { get; set; }
        [JsonProperty("logoText")]
        public string LogoText { get; set; }
    }
}
