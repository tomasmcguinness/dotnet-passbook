using System;
using System.Linq;
using Newtonsoft.Json;
using Passbook.Generator.Fields;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

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

        #region Standard Keys

        /// <summary>
        /// Required. Pass type identifier, as issued by Apple. The value must correspond with your signing certificate.
        /// </summary>
        public string Identifier { get; set; }
        /// <summary>
        /// Required. Version of the file format. The value must be 1.
        /// </summary>
        public int FormatVersion { get { return 1; } }
        /// <summary>
        /// Required. Serial number that uniquely identifies the pass. No two passes with the same pass type identifier may have the same serial number.
        /// </summary>
        public string SerialNumber { get; set; }
        /// <summary>
        /// A simple description of the pass
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Required. Team identifier of the organization that originated and signed the pass, as issued by Apple.
        /// </summary>
        public string TeamIdentifier { get; set; }
        /// <summary>
        /// Required. Display name of the organization that originated and signed the pass.
        /// </summary>
        public string OrganizationName { get; set; }

        #endregion

        #region Images Files

        /// <summary>
        /// Passbook images folder
        /// Images names and sizes can be found at http://developer.apple.com/library/ios/#documentation/userexperience/Conceptual/PassKit_PG/Chapters/Creating.html#//apple_ref/doc/uid/TP40012195-CH4-SW1
        /// </summary>
        public string ImagesPath { get; set; }
        /// <summary>
        /// Images override from <paramref name="ImagesPath"/> where you specify the file to override and give it's path and filename
        /// </summary>
        public Dictionary<PassbookImage, string> ImagesList { get; set; }

        #endregion

        #region Visual Appearance Keys

        /// <summary>
        /// Optional. Foreground color of the pass, specified as a CSS-style RGB triple. For example, rgb(100, 10, 110).
        /// </summary>
        public object ForegroundColor { get; set; }
        /// <summary>
        /// Optional. Background color of the pass, specified as an CSS-style RGB triple. For example, rgb(23, 187, 82).
        /// </summary>
        public string BackgroundColor { get; set; }
        /// <summary>
        /// Optional. Color of the label text, specified as a CSS-style RGB triple. For example, rgb(255, 255, 255).
        /// If omitted, the label color is determined automatically.
        /// </summary>
        public string LabelColor { get; set; }
        /// <summary>
        /// Optional. Text displayed next to the logo on the pass.
        /// </summary>
        public string LogoText { get; set; }
        /// <summary>
        /// Optional. If true, the strip image is displayed without a shine effect. The default value is false.
        /// </summary>
        public bool SuppressStripeShine { get; set; }

        /// <summary>
        /// Optional. Fields to be displayed prominently on the front of the pass.
        /// </summary>
        public List<Field> HeaderFields { get; private set; }
        /// <summary>
        /// Optional. Fields to be displayed prominently on the front of the pass.
        /// </summary>
        public List<Field> PrimaryFields { get; private set; }
        /// <summary>
        /// Optional. Fields to be displayed on the front of the pass.
        /// </summary>
        public List<Field> SecondaryFields { get; private set; }
        /// <summary>
        /// Optional. Additional fields to be displayed on the front of the pass.
        /// </summary>
        public List<Field> AuxiliaryFields { get; private set; }
        /// <summary>
        /// Optional. Information about fields that are displayed on the back of the pass.
        /// </summary>
        public List<Field> BackFields { get; private set; }

        /// <summary>
        /// Optional. Information specific to barcodes.
        /// </summary>
        public BarCode Barcode { get; private set; }

        /// <summary>
        /// Required. Pass type.
        /// </summary>
        public PassStyle Style { get; set; }
        /// <summary>
        /// Required for boarding passes; otherwise not allowed. Type of transit.
        /// </summary>
        public TransitType TransitType { get; set; }

        #endregion
        
        #region Certificate

        /// <summary>
        /// Certificate Thumbprint value
        /// </summary>
        public string CertThumbprint { get; set; }
        /// <summary>
        /// Certificate Store Location
        /// </summary>
        public StoreLocation CertLocation { get; set; }

        #endregion

        #region Web Service Keys

        /// <summary>
        /// The authentication token to use with the web service.
        /// </summary>
        public string AuthenticationToken { get; set; }
        /// <summary>
        /// The URL of a web service that conforms to the API described in Pass Kit Web Service Reference.
        /// The web service must use the HTTPS protocol and includes the leading https://.
        /// On devices configured for development, there is UI in Settings to allow HTTP web services.
        /// </summary>
        public string WebServiceUrl { get; set; }

        #endregion

        #region Helpers

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

        #endregion
    }
}
