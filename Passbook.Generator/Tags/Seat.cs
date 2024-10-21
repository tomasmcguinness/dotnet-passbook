namespace Passbook.Generator.Tags;

public class Seat
{
    /// <summary>
    /// A description of the seat, such as “A flat bed seat”.
    /// </summary>
    public string SeatDescription { get; set; }

    /// <summary>
    /// The identifier code for the seat.
    /// </summary>

    public string SeatIdentifier { get; set; }


    /// <summary>
    /// The number of the seat.
    /// </summary>
    public string SeatNumber { get; set; }


    /// <summary>
    /// The row that contains the seat.
    /// </summary>
    public string SeatRow { get; set; }

    /// <summary>
    /// The section that contains the seat.
    /// </summary>

    public string SeatSection { get; set; }

    /// <summary>
    /// The type of seat, such as “Reserved seating”.
    /// </summary>
    public string SeatType { get; set; }
}
