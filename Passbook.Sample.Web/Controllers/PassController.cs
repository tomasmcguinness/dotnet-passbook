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
using System.Net;

namespace Passbook.Sample.Web.Controllers
{
    public class PassController : Controller
    {
        public ActionResult EventTicket()
        {
            return View();
        }

        public ActionResult BoardingCard()
        {
            return View();
        }

        public ActionResult Coupon()
        {
            PassGenerator generator = new PassGenerator();

            CouponPassGeneratorRequest request = new CouponPassGeneratorRequest();
            request.PassTypeIdentifier = "pass.passverse.com.public";
            request.Certificate = System.IO.File.ReadAllBytes(@"C:\Development\TestMDM\Certificates\SSL Certificates\SSL.pfx");
            request.CertificatePassword = "Bjaxebh2";
            request.AppleWWDRCACertificate = System.IO.File.ReadAllBytes(@"C:\Users\gtmx\Downloads\AppleIncRootCertificate.cer");
            request.SerialNumber = "121211";
            request.Description = "My first pass";
            request.OrganizationName = "Tomas McGuinness";
            request.TeamIdentifier = "R5QS56362W";
            request.LogoText = "My Pass";
            request.BackgroundColor = "rgb(0,0,0)";
            request.ForegroundColor = "rgb(255,255,255)";

            request.AssociatedStoreIdentifiers.Add(551768476);
            request.AppLaunchURL = "something-goes-here";

            // override icon and icon retina
            request.Images.Add(PassbookImage.Icon, System.IO.File.ReadAllBytes(Server.MapPath("~/Icons/icon.png")));
            request.Images.Add(PassbookImage.IconRetina, System.IO.File.ReadAllBytes(Server.MapPath("~/Icons/icon@2x.png")));

            request.AddBarcode(BarcodeType.PKBarcodeFormatPDF417, "01927847623423234234", "UTF-8", "01927847623423234234");

            byte[] generatedPass = generator.Generate(request);

            return new FileContentResult(generatedPass, "application/vnd.apple.pkpass");
        }
    }
}
