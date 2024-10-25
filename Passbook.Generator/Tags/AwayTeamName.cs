namespace Passbook.Generator.Tags;

/// <summary>
/// The name of the away team. Use this key only for a sports event ticket.
/// </summary>
public class AwayTeamName(string value) : SemanticTagBaseValue("awayTeamName", value)
{
}
