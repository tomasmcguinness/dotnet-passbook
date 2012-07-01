using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Passbook.Generator;

namespace Passbook.Sample.Web.Controllers
{
    public class PassController : Controller
    {
        public ActionResult Index()
        {
            PassGenerator generator = new PassGenerator();

            PassGeneratorRequest request = new PassGeneratorRequest();
            request.Identifier = "pass.tomasmcguinness.com";
            request.CertThumbnail = ConfigurationManager.AppSettings["PassBookCertificateThumbnail"];
            request.FormatVersion = 1;
            request.SerialNumber = "121212";
            request.Description = "My first pass";
            request.OrganizationName = "Tomas McGuinness";
            request.TeamIdentifier = "Team America";
            request.LogoText = "My Pass";
            request.BackgroundColor = "rgb(23, 187, 82)";

            request.IconFile = Server.MapPath(@"icon.png");
            request.IconRetinaFile = Server.MapPath(@"icon@2x.png");

            request.LogoFile = Server.MapPath(@"logo.png");
            request.LogoRetinaFile = Server.MapPath(@"logo@2x.png");

            request.Event = new EventTicket();
            request.Event.EventName = "Amazing Event";
            request.Event.VenueName = "The O2 Arena";

            request.AddBarCode("http://test", BarcodeType.PKBarcodeFormatQR, "iso-8859-1", "BarCode AltText");

            Pass generatedPass = generator.Generate(request);

            return new FileContentResult(generatedPass.GetPackage(), "application/vnd.apple.pkpass");
        }
    }
}
