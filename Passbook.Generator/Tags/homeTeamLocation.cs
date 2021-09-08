namespace Passbook.Generator.Tags
{
    /// <summary>
    /// The home location of the home team. Use this key only for a sports event ticket.
    /// </summary>
    public class HomeTeamLocation : SemanticTagBaseValue
    {
        public HomeTeamLocation(string value) : base("homeTeamLocation", value)
        {
            // NO OP
        }
    }
}
