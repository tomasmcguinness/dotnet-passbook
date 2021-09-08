namespace Passbook.Generator.Tags
{
    /// <summary>
    /// The ticketed passenger’s frequent flyer or loyalty number. Use this key for any type of boarding pass.
    /// </summary>
    public class MembershipProgramNumber : SemanticTagBaseValue
    {
        public MembershipProgramNumber(string value) : base("membershipProgramNumber", value)
        {
        }
    }
}

