using System.IO;

namespace Passbook.SampleWebService.Services
{
    public class PassGeneratorConfiguration
    {
        public string AppleWWDRCACertificatePath { get; set; }

        public string CertificatePath { get; set; }

        public string CertificatePassword { get; set; }

        public string WebServiceUrl { get; set; }

        public string PassTypeIdentifier { get; set; }

        public string TeamIdentifier { get; set; }

        internal bool IsValid()
        {
            return !string.IsNullOrEmpty(AppleWWDRCACertificatePath) &&
                   File.Exists(AppleWWDRCACertificatePath) &&
                   !string.IsNullOrEmpty(CertificatePath) &&
                   File.Exists(CertificatePath) &&
                   !string.IsNullOrEmpty(CertificatePassword) &&
                   !string.IsNullOrEmpty(WebServiceUrl) &&
                   !string.IsNullOrEmpty(PassTypeIdentifier) &&
                   !string.IsNullOrEmpty(TeamIdentifier);
        }
    }
}
