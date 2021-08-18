using Newtonsoft.Json;

namespace Passbook.Generator.Tags
{
    public class AirlineCode : SemanticTagBaseValue
    {
        public AirlineCode(string value) : base("airlineCode", value)
        {
            // NO Op
        }
    }
}
