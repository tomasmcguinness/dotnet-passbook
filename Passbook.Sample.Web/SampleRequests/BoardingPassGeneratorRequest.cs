using Passbook.Generator;
using Passbook.Generator.Fields;

namespace Passbook.Sample.Web.SampleRequests
{
    public class BoardingCardGeneratorRequest : PassGeneratorRequest
    {
        public BoardingCardGeneratorRequest()
        {
            this.Style = PassStyle.BoardingPass;
        }

        public string Origin { get; set; }
        public string OriginCode { get; set; }
        public string Destination { get; set; }
        public string DestinationCode { get; set; }

        public string BoardingGate { get; set; }
        public string Seat { get; set; }
        public string PassengerName { get; set; }

        public override void PopulateFields()
        {
            this.AddPrimaryField(new StandardField("origin", Origin, OriginCode));
            this.AddPrimaryField(new StandardField("destination", Destination, DestinationCode));

            this.AddSecondaryField(new StandardField("boarding-gate", "Gate", BoardingGate));

            this.AddAuxiliaryField(new StandardField("seat", "Seat", Seat));
            this.AddAuxiliaryField(new StandardField("passenger-name", "Passenger", PassengerName));
        }
    }
}
