namespace Passbook.Generator.Tags;

/// <summary>
/// The name of the city or hosting region of the venue. Use this key for any type of event ticket.
/// </summary>
public class VenueRegionName(string value) : SemanticTagBaseValue("venueRegionName", value)
{
}
