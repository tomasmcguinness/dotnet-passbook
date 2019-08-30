using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Passbook.SampleWebService.Models;
using Passbook.SampleWebService.Repository;

namespace Passbook.SampleWebService.Controllers
{
    [ApiController]
    public class PassKitController : ControllerBase
    {
        private readonly IWebServiceHandler _handler;

        public PassKitController(IWebServiceHandler handler)
        {
            _handler = handler;
        }

        /// <summary>
        /// When a pass is opened on a device, this API method will be called. This registers the pass on the device.
        /// </summary>
        /// <param name="model">The body of the request includes the push token, which is used to send notifications.</param>
        /// <param name="version">The version of the API. This is fixed at 1.</param>
        /// <param name="deviceLibraryIdentifier">The identifier of the device</param>
        /// <param name="passTypeIdentifier">The identifier of the pass.</param>
        /// <param name="serialNumber">The serial number of the pass.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{version}/devices/{deviceLibraryIdentifier}/registrations/{passTypeIdentifier}/{serialNumber}")]
        public ActionResult RegisterPass([FromBody] RegistrationRequestModel model,
                                         string version,
                                         string deviceLibraryIdentifier,
                                         string passTypeIdentifier,
                                         string serialNumber)
        {
            var authenticationToken = GetAuthenticationTokenFromHeader(Request);

            var isAuthorized = _handler.IsAuthorized(passTypeIdentifier, serialNumber, authenticationToken);

            if (!isAuthorized)
            {
                return Unauthorized();
            }

            var pushToken = model.PushToken;

            var result = _handler.RegisterPass(version, deviceLibraryIdentifier, passTypeIdentifier, serialNumber, pushToken);

            switch (result)
            {
                case PassRegistrationResult.Registered:
                    return Created("", null);
                case PassRegistrationResult.AlreadyRegistered:
                    return Ok();
                default:
                    return NotFound();
            }
        }

        /// <summary>
        /// The AuthenticationToken will be passed by PassKit in the HTTP Authorization header. It is prefixed with ApplePass.
        /// </summary>
        /// <example>
        /// Authorization: ApplePass <TOKEN>
        /// </example>
        /// <param name="request"></param>
        /// <returns></returns>
        private string GetAuthenticationTokenFromHeader(HttpRequest request)
        {
            request.Headers.TryGetValue("Authorization", out StringValues headerValues);

            if (headerValues.Count == 0)
            {
                return null;
            }
            else
            {
                var authorizationHeaderValue = headerValues[0];

                var tokenValue = authorizationHeaderValue.Replace("ApplePass", "").Trim();

                return tokenValue;
            }
        }
    }
}
