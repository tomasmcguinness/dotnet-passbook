namespace Passbook.Generator.Tags;

/// <summary>
/// The name of a frequent flyer or loyalty program. Use this key for any type of boarding pass.
/// </summary>
public class MembershipProgramName(string value) : SemanticTagBaseValue("membershipProgramName", value)
{
}
