using Newtonsoft.Json;

namespace Passbook.Generator.Tags
{
    public class Duration : SemanticTagBaseValue
    {
        private readonly double _value;

        public Duration(double value) : base("duration", value)
        {
            // NO OP
        }
    }
}
