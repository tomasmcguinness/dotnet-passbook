using Passbook.Generator;
using Passbook.Generator.Fields;
using System;

namespace Passbook.Sample.Web.SampleRequests
{
    public class EventPassGeneratorRequest : PassGeneratorRequest
    {
        public EventPassGeneratorRequest()
        {
            this.Style = PassStyle.EventTicket;
        }

        public string EventName { get; set; }
        public int SeatingSection { get; set; }
        public DateTime DoorsOpen { get; set; }

        public override void PopulateFields()
        {
            this.AddPrimaryField(new StandardField("event-name", "Event", EventName) { TextAlignment = FieldTextAlignment.PKTextAlignmentRight });
            this.AddPrimaryField(new StandardField("event-style", "Event2", EventName) { TextAlignment = FieldTextAlignment.PKTextAlignmentRight });
            this.AddSecondaryField(new DateField("doors-open", "Doors Open", FieldDateTimeStyle.PKDateStyleMedium, FieldDateTimeStyle.PKDateStyleShort, DoorsOpen));
            this.AddSecondaryField(new NumberField("seating-section", "Seating Section", SeatingSection, FieldNumberStyle.PKNumberStyleSpellOut) { TextAlignment = FieldTextAlignment.PKTextAlignmentRight });
        }
    }
}
