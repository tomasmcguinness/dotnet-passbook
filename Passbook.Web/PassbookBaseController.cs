using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Http;
using Passbook.Generator;
using Passbook.Web.Extensions;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;
using System.Net;

namespace Passbook.Web
{
    public class PassbookBaseController : System.Web.Http.ApiController
    {
        private static string mAuthorizationKey;
        private static string mServiceUrl;
        private IEnumerable<PassProvider> mProviders;

        private static string GenerateAuthorizationToken(string passTypeIdentifier, string serialNumber)
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

        protected String AuthorizationKey
        {
            get { return mAuthorizationKey; }
        }

        protected void ValidateAuthorization(AuthenticationHeaderValue authenticationHeader, string passTypeIdentifier, string serialNumber)
        {
            if (authenticationHeader != null &&
                string.Equals(authenticationHeader.Scheme, "ApplePass", StringComparison.OrdinalIgnoreCase) &&
                !string.IsNullOrEmpty(authenticationHeader.Parameter) &&
                !string.IsNullOrEmpty(passTypeIdentifier) && 
                !string.IsNullOrEmpty(serialNumber))
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

        protected PassProvider GetProvider(String passTypeIdentifier)
        {
            PassProvider provider = mProviders.FirstOrDefault(p => p.SupportsPassType(passTypeIdentifier));

            if (provider == null)
                throw new Exceptions.PassProviderException(String.Format("No PassProvider available for \"{0}\".", passTypeIdentifier));

            return provider;
        }

        public PassbookBaseController()
        {
            if (string.IsNullOrEmpty(mAuthorizationKey))
                mAuthorizationKey = System.Configuration.ConfigurationManager.AppSettings["AuthorizationKey"];

            if (string.IsNullOrEmpty(mServiceUrl))
                mServiceUrl = System.Configuration.ConfigurationManager.AppSettings["PassbookServiceUrl"];
            
            if (string.IsNullOrEmpty(mAuthorizationKey))
                throw new System.Configuration.ConfigurationErrorsException("AuthorizationKey is not configured in appSettings.");

            mProviders = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => 
                    a.GetTypes()
                    .Where(t => !t.IsAbstract && t.IsClass && t.IsSubclassOf(typeof(PassProvider))))
                .Select(t => Activator.CreateInstance (t) as PassProvider);
        }

        protected IHttpActionResult GeneratePass(PassProvider provider, string serialNumber)
        {
            PassGeneratorRequest request = provider.GetPass(serialNumber);

            if (request != null)
            {
                // Passbook webService is configured, enable it in the generated pass
                if (!string.IsNullOrEmpty(mServiceUrl))
                {
                    request.WebServiceUrl = mServiceUrl;
                    request.AuthenticationToken = GenerateAuthorizationToken(provider.PassTypeIdentifier, request.SerialNumber);
                }

                PassGenerator passGenerator = new PassGenerator();
                byte[] pass = passGenerator.Generate(request);

                return new PassbookContentResult(pass);
            }

            return null;
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

