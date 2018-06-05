using System;
using System.Web.Http;
using System.Diagnostics;
using System.Text;
using System.Globalization;
using System.Net;

namespace Passbook.Web
{
    [RoutePrefix("api/passbook")]
    public abstract class PassbookController : PassbookBaseController
    {
        private string GenerateRequestHash(string passTypeIdentifier, string serialNumber, string validity)
        {
            using (System.Security.Cryptography.SHA1CryptoServiceProvider hasher = new System.Security.Cryptography.SHA1CryptoServiceProvider())
            {
                byte[] data = Encoding.UTF8.GetBytes(passTypeIdentifier.ToLower() + serialNumber.ToLower() + validity.ToLower() + AuthorizationKey);
                return BitConverter.ToString(hasher.ComputeHash(data)).Replace("-", string.Empty).ToLower();
            }
        }

        private Boolean ValidateRequestHash(string passTypeIdentifier, string serialNumber, string validity, string validationHash)
        {
            string hash = GenerateRequestHash(passTypeIdentifier, serialNumber, validity);

            if (string.Equals(hash, validationHash, StringComparison.OrdinalIgnoreCase))
            {
                DateTime value;

                // Verify if the requst URL has not expired
                if (DateTime.TryParseExact(validity, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out value) &&
                    DateTime.UtcNow < value)
                    return true;
            }

            Trace.TraceWarning("ValidateRequestHash for [{0}, {1}, {2}]: Mismatch {4} vs {5}.", passTypeIdentifier, serialNumber, validity, validationHash, hash);

            return false;
        }

        public PassbookController() : base()
        {
        }

        [HttpGet]
        [Route("download/{passTypeIdentifier}/{serialNumber}/{validity}/{validationHash}")]
        public IHttpActionResult DownloadPass(string passTypeIdentifier, string serialNumber, string validity, string validationHash)
        {
            if (!ValidateRequestHash(passTypeIdentifier, serialNumber, validity, validationHash))
                return Content<Serialization.ApiResult>(HttpStatusCode.Forbidden, new Serialization.ApiResult("Validation failed"));

            PassProvider provider = GetProvider(passTypeIdentifier);

            try
            {
                IHttpActionResult result = GeneratePass(provider, serialNumber);

                if (result != null)
                    return result;

                Trace.TraceError("DownloadPass: No pass available for [{0}, {1}]", passTypeIdentifier, serialNumber);

                return Content<Serialization.ApiResult>(HttpStatusCode.NoContent, new Serialization.ApiResult("No pass available."));
            }
            catch (System.Data.Common.DbException ex)
            {
                // Do not expose possible sensitive database information from the PassProvider
                Trace.TraceError("DownloadPass for [{0}, {1}]: {2}", passTypeIdentifier, serialNumber, ex.Message);
                return Content<Serialization.ApiResult>(HttpStatusCode.InternalServerError, new Serialization.ApiResult("Database Error"));
            }
            catch (Exception ex)
            {
                Trace.TraceError("DownloadPass for [{0}, {1}]: {2}", passTypeIdentifier, serialNumber, ex.Message);
                return Content<Serialization.ApiResult>(HttpStatusCode.InternalServerError, new Serialization.ApiResult(ex.Message));
            }
        }
    }
}

