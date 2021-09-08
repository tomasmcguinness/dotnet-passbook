namespace Passbook.Generator.Tags
{
    /// <summary>
    /// The priority status the ticketed passenger holds, such as “Gold” or “Silver”. Use this key for any type of boarding pass.
    /// </summary>
    public class PriorityStatus : SemanticTagBaseValue
    {
        public PriorityStatus(string value) : base("priorityStatus", value)
        {
            // NO OP
        }
    }
}
