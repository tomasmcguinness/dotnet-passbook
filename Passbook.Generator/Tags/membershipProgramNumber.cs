namespace Passbook.Generator.Tags;

/// <summary>
/// The ticketed passenger’s frequent flyer or loyalty number. Use this key for any type of boarding pass.
/// </summary>
public class MembershipProgramNumber(string value) : SemanticTagBaseValue("membershipProgramNumber", value)
{
}

