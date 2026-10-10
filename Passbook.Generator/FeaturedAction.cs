using System.Text.Json;

namespace Passbook.Generator;

/// <summary>
/// A quick-action link shown below a pass, such as opening a related collection or a membership benefits page.
/// </summary>
public class FeaturedAction
{
    /// <summary>
    /// A unique identifier for the featured action.
    /// </summary>
    public string Identifier { get; set; }

    /// <summary>
    /// The type of the featured action, such as <see cref="FeaturedActionType.Place"/> or <see cref="FeaturedActionType.MembershipBenefits"/>.
    /// </summary>
    public FeaturedActionType Type { get; set; }

    /// <summary>
    /// The URL the system opens when someone selects the featured action.
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// Optional. SF Symbol name for the action's icon (e.g. "mappin.and.ellipse"). Apple's docs describe every
    /// featured action as having an icon, string, and action, but published examples only show Apple choosing
    /// the icon from <see cref="Type"/> — this is untested and included for experimentation, e.g. with
    /// <see cref="FeaturedActionType.Place"/>, which does not render with just identifier/type/url.
    /// </summary>
    public string Icon { get; set; }

    public void Write(Utf8JsonWriter writer)
    {
        writer.WriteStartObject();

        writer.WritePropertyName("identifier");
        writer.WriteStringValue(Identifier);

        writer.WritePropertyName("type");
        writer.WriteStringValue(Type.ToWireValue());

        writer.WritePropertyName("url");
        writer.WriteStringValue(Url);

        if (!string.IsNullOrEmpty(Icon))
        {
            writer.WritePropertyName("icon");
            writer.WriteStringValue(Icon);
        }

        writer.WriteEndObject();
    }
}

/// <summary>
/// Apple's predefined semantic action types for a <see cref="FeaturedAction"/>. Apple supplies the icon and
/// localized call-to-action text based on the type; a pass may declare at most two featured actions.
/// </summary>
public enum FeaturedActionType
{
    /// <summary>
    /// View Schedule.
    /// </summary>
    ViewSchedule,
    /// <summary>
    /// Watch Trailer.
    /// </summary>
    WatchTrailer,
    /// <summary>
    /// Listen to Music.
    /// </summary>
    ListenToMusic,
    /// <summary>
    /// Call. Requires <see cref="FeaturedAction.Url"/> to be a "tel:" URI (e.g. "tel:+15555551234") — a plain
    /// "https://" URL does not render.
    /// </summary>
    Call,
    /// <summary>
    /// Go to Location.
    /// </summary>
    /// <remarks>
    /// As of the iOS 27 betas, this type does not render on-device with any URL tried ("https://", "maps://",
    /// address- and coordinate-based Apple Maps URLs) and is unaffected by populating
    /// <see cref="PassGeneratorRequest.RelevantLocations"/>. Treat it as non-functional until Apple ships a
    /// working build or publishes the expected payload shape.
    /// </remarks>
    Place,
    /// <summary>
    /// Add to Balance.
    /// </summary>
    AddToBalance,
    /// <summary>
    /// Order Delivery or Pickup.
    /// </summary>
    Order,
    /// <summary>
    /// Shop Online or In-App.
    /// </summary>
    Shop,
    /// <summary>
    /// View Membership Benefits.
    /// </summary>
    MembershipBenefits,
    /// <summary>
    /// Book an Appointment.
    /// </summary>
    BookAppointment,
    /// <summary>
    /// Book a Car.
    /// </summary>
    BookCar,
    /// <summary>
    /// Book a Flight.
    /// </summary>
    BookFlight,
    /// <summary>
    /// Book a Stay.
    /// </summary>
    BookStay,
    /// <summary>
    /// View Offers and Rewards.
    /// </summary>
    ViewOffersRewards
}

public static class FeaturedActionTypeExtensions
{
    /// <summary>
    /// Converts to the camelCase string PassKit expects for the "type" key in pass.json.
    /// </summary>
    public static string ToWireValue(this FeaturedActionType type)
    {
        return type switch
        {
            FeaturedActionType.ViewSchedule => "viewSchedule",
            FeaturedActionType.WatchTrailer => "watchTrailer",
            FeaturedActionType.ListenToMusic => "listenToMusic",
            FeaturedActionType.Call => "call",
            FeaturedActionType.Place => "place",
            FeaturedActionType.AddToBalance => "addToBalance",
            FeaturedActionType.Order => "order",
            FeaturedActionType.Shop => "shop",
            FeaturedActionType.MembershipBenefits => "membershipBenefits",
            FeaturedActionType.BookAppointment => "bookAppointment",
            FeaturedActionType.BookCar => "bookCar",
            FeaturedActionType.BookFlight => "bookFlight",
            FeaturedActionType.BookStay => "bookStay",
            FeaturedActionType.ViewOffersRewards => "viewOffersRewards",
            _ => throw new System.NotImplementedException($"Unknown FeaturedActionType: {type}"),
        };
    }
}
