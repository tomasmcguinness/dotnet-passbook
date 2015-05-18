﻿using Passbook.Generator;
using Passbook.Generator.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using Passbook.Generator.Configuration;

namespace Passbook.Sample.Web.SampleRequests
{
    public class EventPassGeneratorRequest : PassGeneratorRequest
    {
        public EventPassGeneratorRequest()
        {
            this.Style = PassStyle.EventTicket;

			Passbook.Generator.Configuration.TemplateModel model = new Passbook.Generator.Configuration.TemplateModel();

			model.AddField("AuxField1", FieldAttribute.Value, "Aux Value 1");
		
			LoadTemplate ("test", model);
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
