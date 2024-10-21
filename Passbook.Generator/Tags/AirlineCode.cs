namespace Passbook.Generator.Tags;

/// <summary>
/// The IATA airline code, such as “EX” for flightCode “EX123”. Use this key only for airline boarding passes.
/// </summary>
public class AirlineCode(string value) : SemanticTagBaseValue("airlineCode", value)
{
}
