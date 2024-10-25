namespace Passbook.Generator.Tags;

/// <summary>
/// The unique abbreviation of the away team’s name. Use this key only for a sports event ticket.
/// </summary>
public class AwayTeamAbbreviation(string value) : SemanticTagBaseValue("awayTeamAbbreviation", value)
{
}
