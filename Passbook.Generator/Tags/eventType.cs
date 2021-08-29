using Newtonsoft.Json;
using System;

namespace Passbook.Generator.Tags
{
    public class EventType : SemanticTag
    {
        private readonly EventTypes _eventType;

        public EventType(EventTypes eventType) : base("eventType")
        {
            _eventType = eventType;
        }

        public override void WriteValue(JsonWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
