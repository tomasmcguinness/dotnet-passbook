namespace Passbook.Generator.Tags
{
    public class FlightCode : SemanticTagBaseValue
    {
        /// <summary>
        /// The IATA flight code, such as “EX123”. Use this key only for airline boarding passes.
        /// </summary>
        /// <param name="value"></param>
        public FlightCode(string value) : base("flightCode", value)
        {
            // NO OP
        }
    }
}
