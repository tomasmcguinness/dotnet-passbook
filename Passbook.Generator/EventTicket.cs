using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Passbook.Generator
{
    public class EventTicket
    {
        [JsonProperty("primaryFields")]
        public List<Dictionary<string, string>> PrimaryFields { get; set; }
    }
}
