namespace Passbook.Generator.Tags;

/// <summary>
/// The numeric portion of the IATA flight code, such as 123 for flightCode “EX123”. Use this key only for airline boarding passes.
/// </summary>
public class FlightNumber(string value) : SemanticTagBaseValue("flightNumber", value)
{
}
