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
            return View();

            PassGenerator generator = new PassGenerator();

            CouponPassGeneratorRequest request = new CouponPassGeneratorRequest();
            request.PassTypeIdentifier = "pass.passverse.com.public";
            request.CertThumbprint = ConfigurationManager.AppSettings["PassBookCertificateThumbprint"];
            request.CertLocation = System.Security.Cryptography.X509Certificates.StoreLocation.LocalMachine;
            request.SerialNumber = "121211";
            request.Description = "My first pass";
            request.OrganizationName = "Tomas McGuinness";
            request.TeamIdentifier = "R5QS56362W";
            request.LogoText = "My Pass";
            request.BackgroundColor = "rgb(0,0,0)";
            request.ForegroundColor = "rgb(255,255,255)";

            request.AssociatedStoreIdentifiers.Add(551768476);

            // override icon and icon retina
            request.Images.Add(PassbookImage.Icon, System.IO.File.ReadAllBytes(Server.MapPath("~/Icons/icon.png")));
            request.Images.Add(PassbookImage.IconRetina, System.IO.File.ReadAllBytes(Server.MapPath("~/Icons/icon@2x.png")));

            request.AddBarCode("01927847623423234234", BarcodeType.PKBarcodeFormatPDF417, "UTF-8", "01927847623423234234");

            byte[] generatedPass = generator.Generate(request);

            return new FileContentResult(generatedPass, "application/vnd.apple.pkpass");
        }
    }
}
