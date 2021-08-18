using Newtonsoft.Json;

namespace Passbook.Generator.Tags
{
    /// <summary>
    /// The IATA airline code, such as “EX” for flightCode “EX123”. Use this key only for airline boarding passes.
    /// </summary>
    public class AirlineCode : SemanticTagBaseValue
    {
        public AirlineCode(string value) : base("airlineCode", value)
        {
            // NO Op
        }
    }
}
