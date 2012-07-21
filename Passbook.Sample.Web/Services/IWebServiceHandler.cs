using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Passbook.Sample.Web.Services
{
  public interface IWebServiceHandler
  {
    void RegisterPass(string version, string deviceLibraryIdentifier, string passTypeIdentifier, string serialNumber, string pushToken, string authorizationToken, out PassRegistrationResult result);
  }
}