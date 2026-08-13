namespace Passbook.Generator;

/// <summary>
/// 
/// </summary>
public enum PassStyle
{
    Generic,
    BoardingPass,
    Coupon,
    EventTicket,
    StoreCard,

    /// <summary>
    /// The poster generic pass style: a full-art-background variant of <see cref="Generic"/> introduced
    /// alongside poster event tickets. Written as the top-level "posterGeneric" key.
    /// </summary>
    PosterGeneric
}
