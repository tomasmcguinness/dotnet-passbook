using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Passbook.Sample.Web.Services
{
  public class WebServiceHandler : IWebServiceHandler
  {
    public void RegisterPass(string version, string deviceLibraryIdentifier, string passTypeIdentifier, string serialNumber, string pushToken, string authorizationToken, out PassRegistrationResult result)
    {
      // Implement your custom handler here.
      result = PassRegistrationResult.SuccessfullyRegistered;
    }
  }
}