using System;
using Passbook.Generator;
using Passbook.Generator.Configuration;
using Passbook.Generator.Fields;

namespace Passbook.Sample.Web
{
    public class EventPassProvider : Passbook.Web.PassProvider
    {
        public override string PassTypeIdentifier
        {
            get { return "pass.tomsamcguinness.events"; }
        }

        public override bool IsUpdating()
        {
            return false;
        }

        public override PassGeneratorRequest GetPass(string serialNumber)
        {
            PassGeneratorRequest request = new PassGeneratorRequest();

            request.PassTypeIdentifier = PassTypeIdentifier;
            request.SerialNumber = Guid.NewGuid().ToString("D");

            TemplateModel parameters = new TemplateModel();

            parameters.AddField("event", FieldAttribute.Value, "Jeff Wayne's War of the Worlds");

            DateTime eventDate = DateTime.Now.AddDays(1);

            parameters.AddField("doors-open", FieldAttribute.Value, new DateTime(eventDate.Year, eventDate.Month, eventDate.Day, 20, 30, 00));
            parameters.AddField("seating-section", FieldAttribute.Value, 10);

            request.AddBarcode(BarcodeType.PKBarcodeFormatPDF417, "01927847623423234234", "iso-8859-1", "01927847623423234234");

            request.LoadTemplate("Event", parameters);

            return request;
        }
    }
}

