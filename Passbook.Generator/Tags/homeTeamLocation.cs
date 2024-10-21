namespace Passbook.Generator.Tags;

/// <summary>
/// The home location of the home team. Use this key only for a sports event ticket.
/// </summary>
public class HomeTeamLocation(string value) : SemanticTagBaseValue("homeTeamLocation", value)
{
}
