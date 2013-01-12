using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Passbook.Generator;
using Passbook.Generator.Fields;
using System.IO;
using Passbook.Sample.Web.SampleRequests;

namespace Passbook.Sample.Web.Controllers
{
    public class PassController : Controller
    {
        public ActionResult EventTicket()
        {
            PassGenerator generator = new PassGenerator();

            EventPassGeneratorRequest request = new EventPassGeneratorRequest();
            request.Identifier = "pass.tomsamcguinness.events";
            request.CertThumbprint = ConfigurationManager.AppSettings["PassBookCertificateThumbprint"];
            request.CertLocation = System.Security.Cryptography.X509Certificates.StoreLocation.LocalMachine;
            request.SerialNumber = "121211";
            request.Description = "My first pass";
            request.OrganizationName = "Tomas McGuinness";
            request.TeamIdentifier = "R5QS56362W";
            request.LogoText = "My Pass";
            request.BackgroundColor = "rgb(255,255,255)";
            request.ForegroundColor = "rgb(0,0,0)";

            // override icon and icon retina
            request.Images.Add(PassbookImage.Icon, System.IO.File.ReadAllBytes(Server.MapPath("~/Icons/icon.png")));
            request.Images.Add(PassbookImage.IconRetina, System.IO.File.ReadAllBytes(Server.MapPath("~/Icons/icon@2x.png")));

            request.EventName = "Jeff Wayne's War of the Worlds";
            request.SeatingSection = 10;
            request.DoorsOpen = new DateTime(2012, 11, 03, 11, 30, 00);

            request.AddBarCode("01927847623423234234", BarcodeType.PKBarcodeFormatPDF417, "UTF-8", "01927847623423234234");

            byte[] generatedPass = generator.Generate(request);
            return new FileContentResult(generatedPass, "application/vnd.apple.pkpass");
        }

        public ActionResult BoardingCard()
        {
            PassGenerator generator = new PassGenerator();

            BoardingCardGeneratorRequest request = new BoardingCardGeneratorRequest();
            request.Identifier = "pass.tomsamcguinness.events";
            request.CertThumbprint = ConfigurationManager.AppSettings["PassBookCertificateThumbprint"];
            request.CertLocation = System.Security.Cryptography.X509Certificates.StoreLocation.LocalMachine;
            request.SerialNumber = "121212111";
            request.Description = "My first pass";
            request.OrganizationName = "Tomas McGuinness";
            request.TeamIdentifier = "R5QS56362W";
            request.LogoText = "My Pass";
            request.BackgroundColor = "rgb(255,255,255)";
            request.ForegroundColor = "rgb(0,0,0)";

            // Specific information
            //
            request.Origin = "San Francisco";
            request.OriginCode = "SFO";

            request.Destination = "London";
            request.DestinationCode = "LDN";

            request.Seat = "7A";
            request.BoardingGate = "F12";
            request.PassengerName = "John Appleseed";

            request.TransitType = TransitType.PKTransitTypeAir;

            request.AuthenticationToken = "vxwxd7J8AlNNFPS8k0a0FfUFtq0ewzFdc";
            request.WebServiceUrl = "http://192.168.1.59:82/api/";

            byte[] generatedPass = generator.Generate(request);

            return new FileContentResult(generatedPass, "application/vnd.apple.pkpass");
        }

        public ActionResult Coupon()
        {
            PassGenerator generator = new PassGenerator();

            CouponPassGeneratorRequest request = new CouponPassGeneratorRequest();
            request.Identifier = "pass.tomsamcguinness.events";
            request.CertThumbprint = ConfigurationManager.AppSettings["PassBookCertificateThumbprint"];
            request.CertLocation = System.Security.Cryptography.X509Certificates.StoreLocation.LocalMachine;
            request.SerialNumber = "121211";
            request.Description = "My first pass";
            request.OrganizationName = "Tomas McGuinness";
            request.TeamIdentifier = "R5QS56362W";
            request.LogoText = "My Pass";
            request.BackgroundColor = "rgb(0,0,0)";
            request.ForegroundColor = "rgb(255,255,255)";

            // override icon and icon retina
            request.Images.Add(PassbookImage.Icon, System.IO.File.ReadAllBytes(Server.MapPath("~/Icons/icon.png")));
            request.Images.Add(PassbookImage.IconRetina, System.IO.File.ReadAllBytes(Server.MapPath("~/Icons/icon@2x.png")));

            request.AddBarCode("01927847623423234234", BarcodeType.PKBarcodeFormatPDF417, "UTF-8", "01927847623423234234");

            byte[] generatedPass = generator.Generate(request);

            return new FileContentResult(generatedPass, "application/vnd.apple.pkpass");
        }
    }
}
