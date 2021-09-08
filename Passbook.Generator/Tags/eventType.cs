using Newtonsoft.Json;

namespace Passbook.Generator.Tags
{
    /// <summary>
    /// The type of event. Use this key for any type of event ticket.
    /// </summary>
    public class EventType : SemanticTag
    {
        private readonly EventTypes _eventType;

        public EventType(EventTypes eventType) : base("eventType")
        {
            _eventType = eventType;
        }

        public override void WriteValue(JsonWriter writer)
        {
            writer.WriteValue(_eventType.ToString());
        }
    }
}
