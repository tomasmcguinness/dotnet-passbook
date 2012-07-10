using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Passbook.Sample.Web.Services;

namespace Passbook.Sample.Web.Controllers
{
  public class PassRegistrationController : ApiController
  {
    private IWebServiceHandler handler;

    public PassRegistrationController()
    {
      this.handler = new WebServiceHandler();
    }

    public PassRegistrationController(IWebServiceHandler handler)
    {
      this.handler = handler;
    }

    /// <summary>
    /// This method is called by passes that have just been added to PassBook.
    /// </summary>
    /// <param name="version"></param>
    /// <param name="deviceLibraryIdentifier"></param>
    /// <param name="passTypeIdentifier"></param>
    /// <param name="serialNumber"></param>
    /// <param name="message"></param>
    /// <remarks>
    /// https://webServiceURL/version/devices/deviceLibraryIdentifier/registrations/passTypeIdentifier/serialNumber
    /// </remarks>
    /// <returns></returns>
    public async Task<HttpResponseMessage> Post(string version, string deviceLibraryIdentifier, string passTypeIdentifier, string serialNumber, HttpRequestMessage message)
    {
      string authorizationToken = message.Headers.Authorization.Parameter;

      string jsonBody = await message.Content.ReadAsStringAsync();

      JObject json = JObject.Parse(jsonBody);
      string pushToken = json["pushToken"].Value<string>();

      PassRegistrationResult result;

      this.handler.RegisterPass(version, deviceLibraryIdentifier, passTypeIdentifier, serialNumber, pushToken, authorizationToken, out result);

      HttpStatusCode resultCode = HttpStatusCode.InternalServerError;

      switch (result)
      {
        case PassRegistrationResult.SuccessfullyRegistered:
          resultCode = HttpStatusCode.Created;
          break;
        case PassRegistrationResult.UnAuthorized:
          resultCode = HttpStatusCode.Unauthorized;
          break;
        case PassRegistrationResult.AlreadyRegistered:
          resultCode = HttpStatusCode.OK;
          break;
      }

      return new HttpResponseMessage(resultCode);
    }

    //https://webServiceURL/version/devices/deviceLibraryIdentifier/registrations/passTypeIdentifier?passesUpdatedSince=tag
    public async Task<HttpResponseMessage> Get(string version, string deviceLibraryIdentifier, string passTypeIdentifier, HttpRequestMessage request)
    {
      return new HttpResponseMessage(HttpStatusCode.OK);
    }

    //https://webServiceURL/version/passes/passTypeIdentifier/serialNumber
    public async Task<HttpResponseMessage> GetPass(string version, string passTypeIdentifier, string serialNumber)
    {
      return new HttpResponseMessage(HttpStatusCode.OK);
    }
  }
}
