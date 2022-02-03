using Newtonsoft.Json;

namespace Passbook.Generator.Tags
{
    public class Seats : SemanticTag
    {
        private readonly Seat[] _seats;

        public Seats(params Seat[] seats) : base("seats")
        {
            _seats = seats;
        }

        public override void WriteValue(JsonWriter writer)
        {
            writer.WriteStartArray();
            
            foreach(var seat in _seats)
            {
                writer.WriteStartObject();

                writer.WriteEndObject();
            }

            writer.WriteEndArray();
        }
    }
}
