namespace Passbook.Generator.Tags
{
    /// <summary>
    /// The date and time the event ends. Use this key for any type of event ticket.
    /// </summary>
    public class EventEndDate : SemanticTagBaseValue
    {
        /// <param name="value">ISO 8601 date as string</param>
        public EventEndDate(string value) : base("eventEndDate", value)
        {
            // NO OP
        }
    }
}
