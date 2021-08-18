using Newtonsoft.Json;

namespace Passbook.Generator.Tags
{
    public class Duration : SemanticTagBaseValue
    {
        public Duration(double value) : base("duration", value)
        {
            // NO OP
        }
    }
}
