using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Passbook.Generator
{
    public abstract class PassGeneratorRequest
    {
        public string Identifier { get; set; }
        public int FormatVersion { get; set; }
        public string SerialNumber { get; set; }
        public string Description { get; set; }
        public string TeamIdentifier { get; set; }
        public string OrganizationName { get; set; }

        public string BackgroundFile { get; set; }
        public string BackgroundRetinaFile { get; set; }

        public string IconFile { get; set; }
        public string IconRetinaFile { get; set; }

        public string LogoFile { get; set; }
        public string LogoRetinaFile { get; set; }

        public object ForegroundColor { get; set; }
        public string BackgroundColor { get; set; }
        public string LogoText { get; set; }

        public BarCode Barcode { get; private set; }

        public void AddBarCode(string message, BarcodeType type, string encoding, string altText)
        {
            Barcode = new BarCode();
            Barcode.Type = type;
            Barcode.Message = message;
            Barcode.Encoding = encoding;
            Barcode.AlternateText = altText;
        }

        public string CertThumbnail { get; set; }
        public StoreLocation CertLocation { get; set; }
        public object Title { get; set; }
        public string AuthenticationToken { get; set; }
        public string WebServiceUrl { get; set; }
        public string AuthorizationCode { get; set; }
    }
}
