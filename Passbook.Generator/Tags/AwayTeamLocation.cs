namespace Passbook.Generator.Tags;

/// <summary>
/// The home location of the away team. Use this key only for a sports event ticket.
/// </summary>
public class AwayTeamLocation(string value) : SemanticTagBaseValue("awayTeamLocation", value)
{
}
