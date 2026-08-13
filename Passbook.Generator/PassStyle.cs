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
    /// Seems to have dropped the SecondaryFields & AuxiliaryFields in posterGeneric
    /// </summary>
    PosterGeneric
}
