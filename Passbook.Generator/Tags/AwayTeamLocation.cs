namespace Passbook.Generator.Tags
{
    /// <summary>
    /// The home location of the away team. Use this key only for a sports event ticket.
    /// </summary>
    public class AwayTeamLocation : SemanticTagBaseValue
    {
        public AwayTeamLocation(string value) : base("awayTeamLocationSemanticTag", value)
        {
            // NO OP
        }
    }
}
