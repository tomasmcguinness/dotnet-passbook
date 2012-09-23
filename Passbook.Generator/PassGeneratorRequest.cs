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
        public PassGeneratorRequest()
        {
            HeaderFields = new List<Field>();
            PrimaryFields = new List<Field>();
            SecondaryFields = new List<Field>();
            AuxiliaryFields = new List<Field>();
            BackFields = new List<Field>();
        }

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
        public bool SuppressStripeShine { get; set; }

        public List<Field> HeaderFields { get; private set; }
        public List<Field> PrimaryFields { get; private set; }
        public List<Field> SecondaryFields { get; private set; }
        public List<Field> AuxiliaryFields { get; private set; }
        public List<Field> BackFields { get; private set; }

        public BarCode Barcode { get; private set; }

        public void AddHeaderField(Field field)
        {
            this.HeaderFields.Add(field);
        }

        public void AddPrimaryField(Field field)
        {
            this.PrimaryFields.Add(field);
        }

        public void AddSecondaryField(Field field)
        {
            this.SecondaryFields.Add(field);
        }

        public void AddAuxiliaryField(Field field)
        {
            this.AuxiliaryFields.Add(field);
        }

        public void AddBackField(Field field)
        {
            this.BackFields.Add(field);
        }

        public void AddBarCode(string message, BarcodeType type, string encoding, string altText)
        {
            Barcode = new BarCode();
            Barcode.Type = type;
            Barcode.Message = message;
            Barcode.Encoding = encoding;
            Barcode.AlternateText = altText;
        }

        public PassStyle Style { get; set; }
        public TransitType TransitType { get; set; }

        public string CertThumbprint { get; set; }
        public StoreLocation CertLocation { get; set; }

        public string AuthenticationToken { get; set; }
        public string WebServiceUrl { get; set; }
    }
}
