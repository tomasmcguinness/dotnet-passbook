namespace Passbook.Generator.Tags
{
    /// <summary>
    /// An object that represents the name of the passenger. Use this key for any type of boarding pass.
    /// </summary>
    public class PassengerName : SemanticTagBaseValue
    {
        public PassengerName(string value) : base("passengerName", value)
        {
            // NO OP
        }
    }
}
