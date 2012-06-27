using System;
using System.Collections.Generic;
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
          request.Identifier = "R5QS56362W.pass.tomasmcguinness.com";
          request.FormatVersion = 1;
          request.SerialNumber = "121212";
          request.Description = "My first pass";
          request.OrganizationName = "Tomas McGuinness";
          request.TeamIdentifier = "Team America";

          request.IconFile = @"c:\development\icon.png";
          request.IconRetinaFile = @"c:\development\icon@2x.png";

          request.Event = new EventTicket();
          request.Event.PrimaryFields = new System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, string>>();
          request.Event.PrimaryFields.Add(new System.Collections.Generic.Dictionary<string, string>());
          request.Event.PrimaryFields[0].Add("key", "event-name");
          request.Event.PrimaryFields[0].Add("value", "Amazing Event");

          Pass generatedPass = generator.Generate(request);

          return new FileContentResult(generatedPass.GetPackage(), "application/vnd.apple.pkpass");
        }
    }
}
