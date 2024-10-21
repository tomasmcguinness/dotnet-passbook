namespace Passbook.Generator.Tags;

/// <summary>
/// The abbreviated league name for a sports event. Use this key only for a sports event ticket.
/// </summary>
public class LeagueAbbreviation(string value) : SemanticTagBaseValue("leagueAbbreviation", value)
{
}
