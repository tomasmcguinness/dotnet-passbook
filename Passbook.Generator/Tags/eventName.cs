namespace Passbook.Generator.Tags
{
    /// <summary>
    /// The full name of the event, such as the title of a movie. Use this key for any type of event ticket.
    /// </summary>
    public class EventName : SemanticTagBaseValue
    {
        public EventName(string value) : base("eventName", value)
        {
            // NO OP
        }
    }
}
