namespace Passbook.Generator.Tags;

/// <summary>
/// The name of the home team. Use this key only for a sports event ticket.
/// </summary>
public class HomeTeamName(string value) : SemanticTagBaseValue("homeTeamName", value)
{
}
