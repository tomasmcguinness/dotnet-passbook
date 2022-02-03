namespace Passbook.Generator.Tags
{
    /// <summary>
    /// The name of a frequent flyer or loyalty program. Use this key for any type of boarding pass.
    /// </summary>
    public class MembershipProgramName : SemanticTagBaseValue
    {
        public MembershipProgramName(string value) : base("membershipProgramName", value)
        {
        }
    }
}
