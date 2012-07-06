using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Passbook.Sample.Web.Controllers
{
    public class RegistrationController : ApiController
    {
      //https://webServiceURL/version/devices/deviceLibraryIdentifier/registrations/passTypeIdentifier/serialNumber
      public HttpResponseMessage Post(string version, string deviceLibraryIdentifier, string passTypeIdentifier, string serialNumber)
      {
          return new HttpResponseMessage(HttpStatusCode.Created);
      }
    }
}
