using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Passbook.SampleWebService.Models;

namespace Passbook.SampleWebService.Controllers
{
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private IWebServiceHandler _handler;

        public ValuesController(IWebServiceHandler handler)
        {
            _handler = handler;
        }

        [HttpGet]
        [Route("{version}/devices/{deviceLibraryIdentifier}/registrations/{passTypeIdentifier}/{serialNumber}")]
        public async Task<ActionResult> RegisterPass([FromBody] RegistrationRequestModel model, string version, string deviceLibraryIdentifier, string passTypeIdentifier, string serialNumber)
        {
            var authenticationToken = GetAuthenticationTokenFromHeader(Request);

            var isAuthorized = await _handler.IsAuthorizedAsync(version, deviceLibraryIdentifier, passTypeIdentifier, serialNumber, authenticationToken);

            if (!isAuthorized)
            {
                return Unauthorized();
            }

            var pushToken = model.PushToken;

            var result = await _handler.RegisterPassAsync(version, deviceLibraryIdentifier, passTypeIdentifier, serialNumber, pushToken, "Private");

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
        /// The AuthenticationToken will be passed by PassKit in hte Authorization header.
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

                var tokenValue = authorizationHeaderValue.Replace("ApplePass", "");
                tokenValue = tokenValue.Trim();

                return tokenValue;
            }
        }
    }
}
