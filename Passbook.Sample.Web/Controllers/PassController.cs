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

            EventPassGeneratorRequest request = new EventPassGeneratorRequest();
            request.Identifier = "pass.tomasmcguinness.com";
            request.CertThumbnail = ConfigurationManager.AppSettings["PassBookCertificateThumbnail"];
            request.FormatVersion = 1;
            request.SerialNumber = "121212";
            request.Description = "My first pass";
            request.OrganizationName = "Tomas McGuinness";
            request.TeamIdentifier = "Team America";
            request.LogoText = "My Pass";
            request.BackgroundColor = "rgb(255, 255, 255)";

            request.BackgroundFile = Server.MapPath(@"~/Icons/Starbucks/background.png");
            request.BackgroundRetinaFile = Server.MapPath(@"~/Icons/Starbucks/background@2x.png");

            request.IconFile = Server.MapPath(@"~/Icons/icon.png");
            request.IconRetinaFile = Server.MapPath(@"~/Icons/icon@2x.png");

            request.LogoFile = Server.MapPath(@"~/Icons/logo.png");
            request.LogoRetinaFile = Server.MapPath(@"~/Icons/logo@2x.png");

            request.EventName = "Jeff Wayne's War of the Worlds";
            request.VenueName = "The O2";

            request.AuthenticationToken = "vxwxd7J8AlNNFPS8k0a0FfUFtq0ewzFdc";
            request.WebServiceUrl = "http://192.168.1.89:81/api";

            request.AddBarCode("01927847623423234234", BarcodeType.PKBarcodeFormatPDF417, "UTF-8", "01927847623423234234");

            Pass generatedPass = generator.Generate(request);

            return new FileContentResult(generatedPass.GetPackage(), "application/vnd.apple.pkpass");
        }

        public ActionResult StoreCard()
        {
            PassGenerator generator = new PassGenerator();

            StoreCardGeneratorRequest request = new StoreCardGeneratorRequest();
            request.Identifier = "pass.tomasmcguinness.com";
            request.CertThumbnail = ConfigurationManager.AppSettings["PassBookCertificateThumbnail"];
            request.FormatVersion = 1;
            request.SerialNumber = "121212";
            request.Description = "My first pass";
            request.OrganizationName = "Tomas McGuinness";
            request.TeamIdentifier = "Team America";
            request.LogoText = "My Pass";
            request.BackgroundColor = "#000000";
            request.ForegroundColor = "#FFFFFF";

            request.BackgroundFile = Server.MapPath(@"~/Icons/Starbucks/background.png");
            request.BackgroundRetinaFile = Server.MapPath(@"~/Icons/Starbucks/background@2x.png");

            request.IconFile = Server.MapPath(@"~/Icons/Starbucks/icon.png");
            request.IconRetinaFile = Server.MapPath(@"~/Icons/Starbucks/icon@2x.png");

            request.LogoFile = Server.MapPath(@"~/Icons/Starbucks/logo.png");
            request.LogoRetinaFile = Server.MapPath(@"~/Icons/Starbucks/logo@2x.png");

            // Specific information
            //
            request.Balance = 100.12;
            request.OwnersName = "Tomas McGuinness";
            request.Title = "Starbucks";
            request.AddBarCode("01927847623423234234", BarcodeType.PKBarcodeFormatPDF417, "UTF-8", "01927847623423234234");

            request.AuthenticationToken = "vxwxd7J8AlNNFPS8k0a0FfUFtq0ewzFdc";
            request.WebServiceUrl = "http://192.168.1.89:81/api/";

            Pass generatedPass = generator.Generate(request);

            return new FileContentResult(generatedPass.GetPackage(), "application/vnd.apple.pkpass");
        }
    }
}
