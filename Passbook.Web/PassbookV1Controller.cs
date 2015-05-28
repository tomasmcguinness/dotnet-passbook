using System;
using System.Linq;
using System.Web.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Globalization;
using Passbook.Web.Extensions;

namespace Passbook.Web
{
	/// <summary>
	/// <see cref="PassbookV1Controller" /> implements the v1 specification of the Apple Passbook webservice.
	/// (https://developer.apple.com/library/ios/documentation/PassKit/Reference/PassKit_WebService/WebService.html)
	/// </summary>
	[RoutePrefix("api/passbook/v1")]
	public class PassbookV1Controller : System.Web.Http.ApiController, IDisposable
	{
		private string mAuthorizationKey;
		private IEnumerable<PassProvider> mProviders;

		private string GenerateAuthorizationToken(string passTypeIdentifier, string serialNumber)
		{
			using (System.Security.Cryptography.SHA1CryptoServiceProvider hasher = new System.Security.Cryptography.SHA1CryptoServiceProvider())
			{
				byte[] data = Encoding.UTF8.GetBytes(passTypeIdentifier.ToLower() + serialNumber.ToLower() + mAuthorizationKey);
				return System.BitConverter.ToString(hasher.ComputeHash(data)).Replace("-", string.Empty).ToLower();
			}
		}

		private bool ValidateAuthorization(string authorizationToken, string passTypeIdentifier, string serialNumber)
		{
			String generatedToken = GenerateAuthorizationToken(passTypeIdentifier, serialNumber);
			return String.Equals(authorizationToken, generatedToken, StringComparison.OrdinalIgnoreCase);
		}

		private void ValidateAuthorization(AuthenticationHeaderValue authenticationHeader, string passTypeIdentifier, string serialNumber)
		{
			if (authenticationHeader != null &&
				String.Equals(authenticationHeader.Scheme, "ApplePass", StringComparison.OrdinalIgnoreCase) &&
				!String.IsNullOrEmpty(authenticationHeader.Parameter) &&
				!String.IsNullOrEmpty(passTypeIdentifier) && 
				!String.IsNullOrEmpty(serialNumber))
			{
				if (ValidateAuthorization(authenticationHeader.Parameter, passTypeIdentifier.ToLower(), serialNumber))
					return;
			}

			// Authorization token could not be validated
			throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden)
			{
				Content = new StringContent("Access Denied", Encoding.UTF8, "text/plain")
			});
		}

		private PassProvider GetProvider(String passTypeIdentifier)
		{
			PassProvider provider = mProviders.FirstOrDefault(p => p.SupportsPassType(passTypeIdentifier));

			if (provider == null)
				throw new Exceptions.PassProviderException(String.Format("No PassProvider available for \"{0}\".", passTypeIdentifier));

			return provider;
		}

		public PassbookV1Controller()
		{
			mAuthorizationKey = System.Configuration.ConfigurationManager.AppSettings["AuthorizationKey"];

			if (String.IsNullOrEmpty(mAuthorizationKey))
				throw new System.Configuration.ConfigurationErrorsException("AuthorizationKey is not configured in appSettings.");

			mProviders = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(a => 
					a.GetTypes()
					.Where(t => !t.IsAbstract && t.IsClass && t.IsSubclassOf(typeof(PassProvider))))
				.Select(t => Activator.CreateInstance (t) as PassProvider);	

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
					return Content<Serialization.ApiResult>(HttpStatusCode.NoContent, new Serialization.ApiResult("No updated device passes available."));

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
				byte[] pass = provider.GetPass(passTypeIdentifier, serialNumber);

				if (pass != null)
					return new PassbookContentResult(pass);

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

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				foreach (PassProvider provider in mProviders)
					provider.Dispose();
			}

			base.Dispose(disposing);
		}
	}
}

