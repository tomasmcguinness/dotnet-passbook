namespace Passbook.Generator.Tags;

/// <summary>
/// An object that represents the name of the passenger. Use this key for any type of boarding pass.
/// </summary>
public class PassengerName(string value) : SemanticTagBaseValue("passengerName", value)
{
}
