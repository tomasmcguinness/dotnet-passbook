using Newtonsoft.Json;

namespace Passbook.Generator.Tags
{
    /// <summary>
    /// An array of the full names of the performers and opening acts at the event, in decreasing order of significance. Use this key for any type of event ticket.
    /// </summary>
    public class PerformerNames : SemanticTag
    {
        private readonly string[] _performerNames;

        public PerformerNames(params string[] performerNames) : base("performerNames")
        {
            _performerNames = performerNames;
        }

        public override void WriteValue(JsonWriter writer)
        {
            writer.WriteValue(_performerNames);
        }
    }
}
