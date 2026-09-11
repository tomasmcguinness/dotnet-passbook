namespace Passbook.Generator;

/// <summary>
/// Style schemes that the system resolves into a style type for the pass, in order of preference.
/// </summary>
public enum PreferredStyleScheme
{
    /// <summary>
    /// A poster-style event ticket. Falls back to <see cref="EventTicket"/> on devices that don't support it.
    /// </summary>
    PosterEventTicket,

    /// <summary>
    /// The legacy event ticket style.
    /// </summary>
    EventTicket,

    /// <summary>
    /// The poster-style generic ticket. Falls back to <see cref="Generic"/> on devices that don't support it.
    /// </summary>
    PosterGeneric,

    /// <summary>
    /// The legacy generic ticket style
    /// </summary>
    Generic
}
