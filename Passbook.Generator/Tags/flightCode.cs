namespace Passbook.Generator.Tags;

/// <summary>
/// The IATA flight code, such as “EX123”. Use this key only for airline boarding passes.
/// </summary>
/// <param name="value"></param>
public class FlightCode(string value) : SemanticTagBaseValue("flightCode", value)
{
}
