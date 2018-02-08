using System;
using Passbook.Generator;
using Passbook.Generator.Configuration;
using Passbook.Generator.Fields;

namespace Passbook.Sample.Web
{
    public class BoardingPassProvider : Passbook.Web.PassProvider
    {
        public override string PassTypeIdentifier
        {
            get { return "pass.tomsamcguinness.travel"; }
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

            parameters.AddField("origin", FieldAttribute.Label, "San Francisco");
            parameters.AddField("origin", FieldAttribute.Value, "SFO");

            parameters.AddField("destination", FieldAttribute.Label, "London Heathrow");
            parameters.AddField("destination", FieldAttribute.Value, "LHR");

            parameters.AddField("seat", FieldAttribute.Value, "7A");
            parameters.AddField("boarding-gate", FieldAttribute.Value, "F12");
            parameters.AddField("passenger-name", FieldAttribute.Value, "John Appleseed");

            request.AddBarcode(BarcodeType.PKBarcodeFormatPDF417, "M1APPLESEED/JMR EZQ7O92 GVALHRBA 00723319C002F00009100", "iso-8859-1");

            request.LoadTemplate("BoardingPass", parameters);

            return request;
        }
    }
}

