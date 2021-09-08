namespace Passbook.Generator.Tags
{
    /// <summary>
    /// The abbreviated league name for a sports event. Use this key only for a sports event ticket.
    /// </summary>
    public class LeagueAbbreviation : SemanticTagBaseValue
    {
        public LeagueAbbreviation(string value) : base("leagueAbbreviation", value)
        {
        }
    }
}
