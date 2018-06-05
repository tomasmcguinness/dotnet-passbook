using System;
using System.Linq;
using System.Web.Http;
using System.Net.Http;
using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Globalization;

namespace Passbook.Web
{
    /// <summary>
    /// <see cref="PassbookV1Controller" /> implements the v1 specification of the Apple Passbook webservice.
    /// (https://developer.apple.com/library/ios/documentation/PassKit/Reference/PassKit_WebService/WebService.html)
    /// </summary>
    [RoutePrefix("api/passbook/v1")]
    public class PassbookV1Controller : PassbookBaseController
    {
        public PassbookV1Controller() : base()
        {
        }

        /// <summary>
        /// Registers the a device / pass combination.
        /// </summary>
        /// <remarks>
        /// deviceLibraryIdentifier
        /// A unique identifier that is used to identify and authenticate this device in future requests.
        /// - passTypeIdentifier
        /// The pass’s type, as specified in the pass.
        /// - serialNumber
        /// The pass’s serial number, as specified in the pass.
        /// - Header
        /// The Authorization header is supplied; its value is the word "ApplePass", followed by a space, 
        /// followed by the pass’s authorization token as specified in the pass.		
        /// - Payload	
        /// The POST payload is a JSON dictionary, containing a single key and value:
        /// * pushToken
        /// The push token that the server can use to send push notifications to this device.
        /// 
        /// Response
        /// * If the serial number is already registered for this device, return HTTP status 200.
        /// * If registration succeeds, return HTTP status 201.
        /// * If the request is not authorized, return HTTP status 401.
        /// * Otherwise, return the appropriate standard HTTP status.
        /// </remarks>
        [HttpPost]
        [Route("devices/{deviceLibraryIdentifier}/registrations/{passTypeIdentifier}/{serialNumber}")]
        public IHttpActionResult RegisterDevicePass(string deviceLibraryIdentifier, string passTypeIdentifier, string serialNumber)
        {
            ValidateAuthorization(Request.Headers.Authorization, passTypeIdentifier, serialNumber);

            PassProvider provider = GetProvider(passTypeIdentifier);

            try
            {
                String body = Request.Content.ReadAsStringAsync().Result;

                if (!String.IsNullOrEmpty(body))
                {
                    JObject json = JObject.Parse(body);
                    JToken token = json.SelectToken("pushToken");

                    if (token != null)
                    {
                        // Register device
                        String pushToken = token.Value<String>();

                        provider.RegisterDevicePass(deviceLibraryIdentifier, passTypeIdentifier, serialNumber, pushToken);

                        return Ok<Serialization.ApiResult>(new Serialization.ApiResult("Pass registered"));
                    }
                }

                Trace.TraceError("RegisterDevicePass for [{0}, {1}, {2}]: Incomplete or invalid body.", deviceLibraryIdentifier, passTypeIdentifier, serialNumber);
                return Content<Serialization.ApiResult>(HttpStatusCode.InternalServerError, new Serialization.ApiResult("Incomplete or invalid body"));
            }
            catch (System.Data.Common.DbException ex)
            {
                // Do not expose possible sensitive database information from the PassProvider
                Trace.TraceError("RegisterDevicePass for [{0}, {1}, {2}]: {3}", deviceLibraryIdentifier, passTypeIdentifier, serialNumber, ex.Message);
                return Content<Serialization.ApiResult>(HttpStatusCode.InternalServerError, new Serialization.ApiResult("Database Error"));
            }
            catch (Exception ex)
            {
                Trace.TraceError("RegisterDevicePass for [{0}, {1}, {2}]: {3}", deviceLibraryIdentifier, passTypeIdentifier, serialNumber, ex.Message);
                return Content<Serialization.ApiResult>(HttpStatusCode.InternalServerError, new Serialization.ApiResult(ex.Message));
            }
        }

        /// <summary>
        /// Retrieve passes registered for a device
        /// </summary>
        /// <remarks>			
        /// - deviceLibraryIdentifier
        /// A unique identifier that is used to identify and authenticate the device.
        /// - passTypeIdentifier
        /// The pass's type, as specified in the pass.
        /// - tag
        /// A tag from a previous request. (optional)
        /// If the passesUpdatedSince parameter is present, return only the passes that have been updated since the time indicated by tag. Otherwise, return all passes.
        /// Response		
        /// If there are matching passes, return HTTP status 200 with a JSON dictionary with the following keys and values:
        /// - lastUpdated (string)
        /// The current modification tag.
        /// - serialNumbers (array of strings)
        /// The serial numbers of the matching passes.
        /// If there are no matching passes, return HTTP status 204.
        /// Otherwise, return the appropriate standard HTTP status.
        /// </remarks>
        [HttpGet]
        [Route("devices/{deviceLibraryIdentifier}/registrations/{passTypeIdentifier}/{passesUpdatedSince?}")]
        public IHttpActionResult GetDevicePasses(string deviceLibraryIdentifier, string passTypeIdentifier, string passesUpdatedSince = null)
        {
            List<String> passes = null;

            PassProvider provider = GetProvider(passTypeIdentifier);

            try
            {
                DateTime updatedSince;

                if (!DateTime.TryParseExact(passesUpdatedSince, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out updatedSince))
                    updatedSince = DateTime.MinValue;

                passes = provider.GetDevicePasses(deviceLibraryIdentifier, passTypeIdentifier, updatedSince);

                if (passes == null || !passes.Any())
                {
                    return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
                }

                return Content<Serialization.UpdatedPasses>(HttpStatusCode.OK,
                    new Serialization.UpdatedPasses()
                    {
                        lastUpdated = DateTime.UtcNow.ToString("yyyyMMdd"),
                        serialNumbers = passes
                    });
            }
            catch (System.Data.Common.DbException ex)
            {
                // Do not expose possible sensitive database information from the PassProvider
                Trace.TraceError("GetDevicePasses for [{0}, {1}, {2}]: {3}", deviceLibraryIdentifier, passTypeIdentifier, passesUpdatedSince, ex.Message);
                return Content<Serialization.ApiResult>(HttpStatusCode.InternalServerError, new Serialization.ApiResult("Database Error"));
            }
            catch (Exception ex)
            {
                Trace.TraceError("GetDevicePasses for [{0}, {1}, {2}]: {3}", deviceLibraryIdentifier, passTypeIdentifier, passesUpdatedSince, ex.Message);
                return Content<Serialization.ApiResult>(HttpStatusCode.InternalServerError, new Serialization.ApiResult(ex.Message));
            }
        }

        /// <summary>
        /// Retrieve the latest pass
        /// </summary>
        /// <param name="passTypeIdentifier">The pass type identifier.</param>
        /// <param name="serialNumber">The serial number.</param>
        /// <returns></returns>
        /// <remarks>
        /// - passTypeIdentifier
        /// The pass's type, as specified in the pass.
        /// - serialNumber
        /// The unique pass identifier, as specified in the pass.
        /// - Header
        /// The Authorization header is supplied; its value is the word “ApplePass”, followed by a space, followed by the pass’s authorization token as specified in the pass.
        /// Response
        ///	If request is authorized, return HTTP status 200 with a payload of the pass data.
        /// If the request is not authorized, return HTTP status 401.
        /// Otherwise, return the appropriate standard HTTP status.
        /// </remarks>
        [HttpGet]
        [Route("passes/{passTypeIdentifier}/{serialNumber}")]
        public IHttpActionResult GetPass(string passTypeIdentifier, string serialNumber)
        {
            ValidateAuthorization(Request.Headers.Authorization, passTypeIdentifier, serialNumber);

            PassProvider provider = GetProvider(passTypeIdentifier);

            try
            {
                IHttpActionResult result = GeneratePass(provider, serialNumber);

                if (result != null)
                    return result;

                Trace.TraceError("GetPass: No pass available for [{0}, {1}]", passTypeIdentifier, serialNumber);

                return Content<Serialization.ApiResult>(HttpStatusCode.NoContent, new Serialization.ApiResult("No pass available."));
            }
            catch (System.Data.Common.DbException ex)
            {
                // Do not expose possible sensitive database information from the PassProvider
                Trace.TraceError("GetPass for [{0}, {1}]: {2}", passTypeIdentifier, serialNumber, ex.Message);
                return Content<Serialization.ApiResult>(HttpStatusCode.InternalServerError, new Serialization.ApiResult("Database Error"));
            }
            catch (Exception ex)
            {
                Trace.TraceError("GetPass for [{0}, {1}]: {2}", passTypeIdentifier, serialNumber, ex.Message);
                return Content<Serialization.ApiResult>(HttpStatusCode.InternalServerError, new Serialization.ApiResult(ex.Message));
            }
        }

        [HttpDelete]
        [Route("devices/{deviceLibraryIdentifier}/registrations/{passTypeIdentifier}/{serialNumber}")]
        public IHttpActionResult UnregisterPass(string deviceLibraryIdentifier, string passTypeIdentifier, string serialNumber)
        {
            ValidateAuthorization(Request.Headers.Authorization, passTypeIdentifier, serialNumber);

            PassProvider provider = GetProvider(passTypeIdentifier);

            try
            {
                provider.UnregisterDevicePass(deviceLibraryIdentifier, passTypeIdentifier, serialNumber);

                return Ok<Serialization.ApiResult>(new Serialization.ApiResult("Unregistered pass"));
            }
            catch (System.Data.Common.DbException ex)
            {
                // Do not expose possible sensitive database information from the PassProvider
                Trace.TraceError("UnregisterPass for [{0}, {1}, {2}]: {3}", deviceLibraryIdentifier, passTypeIdentifier, serialNumber, ex.Message);
                return Content<Serialization.ApiResult>(HttpStatusCode.InternalServerError, new Serialization.ApiResult("Database Error"));
            }
            catch (Exception ex)
            {
                Trace.TraceError("UnregisterPass for [{0}, {1}, {2}]: {3}", deviceLibraryIdentifier, passTypeIdentifier, serialNumber, ex.Message);
                return Content<Serialization.ApiResult>(HttpStatusCode.InternalServerError, new Serialization.ApiResult(ex.Message));
            }
        }

        [HttpPost]
        [Route("log")]
        public IHttpActionResult Log(Serialization.DeviceLogs deviceLogs)
        {
            try
            {
                foreach (String logMessage in deviceLogs.logs)
                    Debug.WriteLine(logMessage);

                string log = string.Join("\r\n", deviceLogs.logs);

                Trace.TraceInformation("Device logs\n{0}", log);
                return Ok<Serialization.ApiResult>(new Serialization.ApiResult("Logged"));
            }
            catch (Exception ex)
            {
                Trace.TraceError("Log: {0}", ex.Message);
                return Content<Serialization.ApiResult>(HttpStatusCode.InternalServerError, new Serialization.ApiResult(ex.Message));
            }
        }
    }
}

