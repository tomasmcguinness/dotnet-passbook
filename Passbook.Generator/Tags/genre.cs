namespace Passbook.Generator.Tags;

/// <summary>
/// The genre of the performance, such as “Classical”. Use this key for any type of event ticket.
/// </summary>
public class Genre(string value) : SemanticTagBaseValue("genre", value)
{
}
