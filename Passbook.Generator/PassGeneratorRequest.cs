using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using Passbook.Generator.Fields;

namespace Passbook.Generator
{
    public class PassGeneratorRequest
    {
        public PassGeneratorRequest()
        {
            this.HeaderFields = new List<Field>();
            this.PrimaryFields = new List<Field>();
            this.SecondaryFields = new List<Field>();
            this.AuxiliaryFields = new List<Field>();
            this.BackFields = new List<Field>();

            this.ImagesList = new Dictionary<PassbookImage, string>();
        }

        public string Identifier { get; set; }
        public int FormatVersion { get; set; }
        public string SerialNumber { get; set; }
        public string Description { get; set; }
        public string TeamIdentifier { get; set; }
        public string OrganizationName { get; set; }

        /// <summary>
        /// Passbook images folder
        /// Images names and sizes can be found at http://developer.apple.com/library/ios/#documentation/userexperience/Conceptual/PassKit_PG/Chapters/Creating.html#//apple_ref/doc/uid/TP40012195-CH4-SW1
        /// </summary>
        public string ImagesPath { get; set; }
        /// <summary>
        /// Images override from <paramref name="ImagesPath"/> where you specify the file to override and give it's path and filename
        /// </summary>
        public Dictionary<PassbookImage, string> ImagesList { get; set; }

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

        public virtual void PopulateFields()
        {
            // NO OP.
        }

        internal void Write(JsonWriter writer)
        {
            PopulateFields();

            writer.WriteStartObject();

            WriteStandardKeys(writer, this);
            WriteAppearanceKeys(writer, this);

            OpenStyleSpecificKey(writer, this);

            WriteSection(writer, "headerFields", this.HeaderFields);
            WriteSection(writer, "primaryFields", this.PrimaryFields);
            WriteSection(writer, "secondaryFields", this.SecondaryFields);
            WriteSection(writer, "auxiliaryFields", this.AuxiliaryFields);
            WriteSection(writer, "backFields", this.BackFields);

            if (this.Style == PassStyle.BoardingPass)
            {
                writer.WritePropertyName("transitType");
                writer.WriteValue(this.TransitType.ToString());
            }

            CloseStyleSpecificKey(writer);

            WriteBarcode(writer, this);
            WriteUrls(writer, this);

            writer.WriteEndObject();
        }

        private void WriteUrls(JsonWriter writer, PassGeneratorRequest request)
        {
            if (!string.IsNullOrEmpty(request.AuthenticationToken))
            {
                writer.WritePropertyName("authenticationToken");
                writer.WriteValue(request.AuthenticationToken);
                writer.WritePropertyName("webServiceURL");
                writer.WriteValue(request.WebServiceUrl);
            }
        }

        private void WriteBarcode(JsonWriter writer, PassGeneratorRequest request)
        {
            if (Barcode != null)
            {
                writer.WritePropertyName("barcode");

                writer.WriteStartObject();
                writer.WritePropertyName("format");
                writer.WriteValue(request.Barcode.Type.ToString());
                writer.WritePropertyName("message");
                writer.WriteValue(request.Barcode.Message);
                writer.WritePropertyName("messageEncoding");
                writer.WriteValue(request.Barcode.Encoding);
                writer.WritePropertyName("altText");
                writer.WriteValue(request.Barcode.AlternateText);
                writer.WriteEndObject();
            }
        }

        private void WriteStandardKeys(JsonWriter writer, PassGeneratorRequest request)
        {
            writer.WritePropertyName("passTypeIdentifier");
            writer.WriteValue(request.Identifier);

            writer.WritePropertyName("formatVersion");
            writer.WriteValue(request.FormatVersion);

            writer.WritePropertyName("serialNumber");
            writer.WriteValue(request.SerialNumber);

            writer.WritePropertyName("description");
            writer.WriteValue(request.Description);

            writer.WritePropertyName("organizationName");
            writer.WriteValue(request.OrganizationName);

            writer.WritePropertyName("teamIdentifier");
            writer.WriteValue(request.TeamIdentifier);

            writer.WritePropertyName("logoText");
            writer.WriteValue(request.LogoText);
        }

        private void WriteAppearanceKeys(JsonWriter writer, PassGeneratorRequest request)
        {
            writer.WritePropertyName("foregroundColor");
            writer.WriteValue(request.ForegroundColor);

            writer.WritePropertyName("backgroundColor");
            writer.WriteValue(request.BackgroundColor);
        }

        private void OpenStyleSpecificKey(JsonWriter writer, PassGeneratorRequest request)
        {
            switch (request.Style)
            {
                case PassStyle.EventTicket:
                    writer.WritePropertyName("eventTicket");
                    writer.WriteStartObject();
                    break;
                case PassStyle.StoreCard:
                    writer.WritePropertyName("eventTicket");
                    writer.WriteStartObject();
                    break;
                case PassStyle.BoardingPass:
                    writer.WritePropertyName("boardingPass");
                    writer.WriteStartObject();
                    break;
                case PassStyle.Generic:
                    writer.WritePropertyName("generic");
                    writer.WriteStartObject();
                    break;
                case PassStyle.Coupon:
                    writer.WritePropertyName("coupon");
                    writer.WriteStartObject();
                    break;
                default:
                    throw new InvalidOperationException("Unsupported pass style specified");
            }
        }

        private void CloseStyleSpecificKey(JsonWriter writer)
        {
            writer.WriteEndObject();
        }

        private void WriteSection(JsonWriter writer, string sectionName, List<Field> fields)
        {
            writer.WritePropertyName(sectionName);
            writer.WriteStartArray();

            foreach (var field in fields)
            {
                field.Write(writer);
            }

            writer.WriteEndArray();
        }
    }
}
