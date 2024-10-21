namespace Passbook.Generator.Tags;

/// <summary>
/// The date and time the event ends. Use this key for any type of event ticket.
/// </summary>
/// <param name="value">ISO 8601 date as string</param>
public class EventEndDate(string value) : SemanticTagBaseValue("eventEndDate", value)
{
}
