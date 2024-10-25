namespace Passbook.Generator.Tags;

/// <summary>
/// The unabbreviated league name for a sports event. Use this key only for a sports event ticket.
/// </summary>
public class LeagueName(string value) : SemanticTagBaseValue("leagueName", value)
{
}
