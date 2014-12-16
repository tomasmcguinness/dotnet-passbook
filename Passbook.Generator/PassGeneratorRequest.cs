using System;
using System.Linq;
using Newtonsoft.Json;
using Passbook.Generator.Fields;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Drawing;
using System.Diagnostics;

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
            this.Images = new Dictionary<PassbookImage, byte[]>();
            this.RelevantLocations = new List<RelevantLocation>();
            this.RelevantBeacons = new List<RelevantBeacon>();
            this.AssociatedStoreIdentifiers = new List<int>();
        }

        #region Standard Keys

        /// <summary>
        /// Required. Pass type identifier, as issued by Apple. The value must correspond with your signing certificate.
        /// </summary>
        public string PassTypeIdentifier { get; set; }
        /// <summary>
        /// Required. Version of the file format. The value must be 1.
        /// </summary>
        public int FormatVersion { get { return 1; } }
        /// <summary>
        /// Required. Serial number that uniquely identifies the pass. No two passes with the same pass type identifier may have the same serial number.
        /// </summary>
        public string SerialNumber { get; set; }
        /// <summary>
        /// Required. A simple description of the pass
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

        #region "Companion App Keys"

        #endregion

        #region "Expiration Keys"

        public DateTime? ExpirationDate { get; set; }

        public Boolean? Voided { get; set; }

        #endregion

        #region Images Files

        /// <summary>
        /// When using in memory, the binary of each image is put here.
        /// </summary>
        public Dictionary<PassbookImage, byte[]> Images { get; set; }

        #endregion

        #region Visual Appearance Keys

        /// <summary>
        /// Optional. Foreground color of the pass, specified as a CSS-style RGB triple. For example, rgb(100, 10, 110).
        /// </summary>
        public string ForegroundColor { get; set; }

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
        public bool? SuppressStripShine { get; set; }

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

        /// <summary>
        /// Optional for event tickets and boarding passes; otherwise not allowed. Identifier used to group related passes
        /// </summary>
        public string GroupingIdentifier { get; set; }

        #endregion

        #region Relevance Keys

        /// <summary>
        /// Optional. Always display the time and date in the given time zone, not in the user’s current time zone. The default value is false
        /// </summary>
        public Boolean? IgnoresTimeZone { get; set; }

        /// <summary>
        /// Optional. Date and time when the pass becomes relevant. For example, the start time of a movie.
        /// </summary>
        public DateTime? RelevantDate { get; set; }

        /// <summary>
        /// Optional. Locations where the passisrelevant. For example, the location of your store.
        /// </summary>
        public List<RelevantLocation> RelevantLocations { get; private set; }

        /// <summary>
        /// Optional. Beacons marking locations where the pass is relevant.
        /// </summary>
        public List<RelevantBeacon> RelevantBeacons { get; private set; }

        /// <summary>
        /// Optional. Maximum distance in meters from a relevant latitude and longitude that the pass is relevant
        /// </summary>
        public int? MaxDistance { get; set; }

        #endregion

        #region Certificate

        /// <summary>
        /// A byte array containing the X509 certificate
        /// </summary>
        public byte[] Certificate { get; set; }

        /// <summary>
        /// A byte array containing the Apple WWDRCA X509 certificate
        /// </summary>
        public byte[] AppleWWDRCACertificate { get; set; }

        /// <summary>
        /// The private key password for the certificate.
        /// </summary>
        public string CertificatePassword { get; set; }

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

        #region Associated App Keys

        public List<int> AssociatedStoreIdentifiers { get; set; }

        public string AppLaunchURL { get; set; }

        #endregion

        #region User Info Keys

        // TODO

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

        public void AddBarCode(string message, BarcodeType type, string encoding)
        {
            Barcode = new BarCode();
            Barcode.Type = type;
            Barcode.Message = message;
            Barcode.Encoding = encoding;
            Barcode.AlternateText = null;
        }

        public void AddLocation(double latitude, double longitude)
        {
            AddLocation(latitude, longitude, null);
        }

        public void AddLocation(double latitude, double longitude, string relevantText)
        {
            this.RelevantLocations.Add(new RelevantLocation() { Latitude = latitude, Longitude = longitude, RelevantText = relevantText });
        }

        public void AddBeacon(string proximityUUID, string relevantText)
        {
            this.RelevantBeacons.Add(new RelevantBeacon() { ProximityUUID = proximityUUID, RelevantText = relevantText });
        }

        public virtual void PopulateFields()
        {
            // NO OP.
        }

        internal void Write(JsonWriter writer)
        {
            PopulateFields();

            writer.WriteStartObject();

            Trace.TraceInformation("Writing standard keys..");
            WriteStandardKeys(writer, this);
            Trace.TraceInformation("Writing relevance keys..");
            WriteRelevanceKeys(writer, this);
            Trace.TraceInformation("Writing appearance keys..");
            WriteAppearanceKeys(writer, this);
            Trace.TraceInformation("Writing expiration keys..");
            WriteExpirationKeys(writer, this);

            Trace.TraceInformation("Opening style section..");
            OpenStyleSpecificKey(writer, this);

            Trace.TraceInformation("Writing header fields");
            WriteSection(writer, "headerFields", this.HeaderFields);
            Trace.TraceInformation("Writing primary fields");
            WriteSection(writer, "primaryFields", this.PrimaryFields);
            Trace.TraceInformation("Writing secondary fields");
            WriteSection(writer, "secondaryFields", this.SecondaryFields);
            Trace.TraceInformation("Writing auxiliary fields");
            WriteSection(writer, "auxiliaryFields", this.AuxiliaryFields);
            Trace.TraceInformation("Writing back fields");
            WriteSection(writer, "backFields", this.BackFields);

            if (this.Style == PassStyle.BoardingPass)
            {
                writer.WritePropertyName("transitType");
                writer.WriteValue(this.TransitType.ToString());
            }

            Trace.TraceInformation("Closing style section..");
            CloseStyleSpecificKey(writer);

            WriteBarcode(writer, this);
            WriteUrls(writer, this);

            writer.WriteEndObject();
        }

        private void WriteRelevanceKeys(JsonWriter writer, PassGeneratorRequest request)
        {
            if (request.RelevantDate.HasValue)
            {
                writer.WritePropertyName("relevantDate");
                writer.WriteValue(request.RelevantDate.Value.ToString("yyyy-MM-ddTHH:mm:sszzz"));
            }

            if (request.MaxDistance.HasValue)
            {
                writer.WritePropertyName("maxDistance");
                writer.WriteValue(request.MaxDistance.Value.ToString());
            }

            if (RelevantLocations.Count > 0)
            {
                writer.WritePropertyName("locations");
                writer.WriteStartArray();

                foreach (var location in RelevantLocations)
                {
                    location.Write(writer);
                }

                writer.WriteEndArray();
            }

            if (RelevantBeacons.Count > 0)
            {
                writer.WritePropertyName("beacons");
                writer.WriteStartArray();

                foreach (var beacon in RelevantBeacons)
                {
                    beacon.Write(writer);
                }

                writer.WriteEndArray();
            }
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

                if (request.Barcode.AlternateText != null)
                {
                    writer.WritePropertyName("altText");
                    writer.WriteValue(request.Barcode.AlternateText);
                }

                writer.WriteEndObject();
            }
        }

        private void WriteStandardKeys(JsonWriter writer, PassGeneratorRequest request)
        {
            writer.WritePropertyName("passTypeIdentifier");
            writer.WriteValue(request.PassTypeIdentifier);

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

            if (request.LogoText != null)
            {
                writer.WritePropertyName("logoText");
                writer.WriteValue(request.LogoText);
            }

            if (this.AssociatedStoreIdentifiers.Count > 0)
            {
                writer.WritePropertyName("associatedStoreIdentifiers");
                writer.WriteStartArray();

                foreach (int storeIdentifier in this.AssociatedStoreIdentifiers)
                {
                    writer.WriteValue(storeIdentifier);
                }

                writer.WriteEndArray();
            }
        }

        private void WriteAppearanceKeys(JsonWriter writer, PassGeneratorRequest request)
        {
            if (request.ForegroundColor != null)
            {
                writer.WritePropertyName("foregroundColor");
                writer.WriteValue(ConvertColor(request.ForegroundColor));
            }

            if (request.BackgroundColor != null)
            {
                writer.WritePropertyName("backgroundColor");
                writer.WriteValue(ConvertColor(request.BackgroundColor));
            }

            if (request.LabelColor != null)
            {
                writer.WritePropertyName("labelColor");
                writer.WriteValue(ConvertColor(request.LabelColor));
            }

            if (request.SuppressStripShine.HasValue)
            {
                writer.WritePropertyName("suppressStripShine");
                writer.WriteValue(request.SuppressStripShine.Value);
            }

            if (request.GroupingIdentifier != null)
            {
                writer.WritePropertyName("groupingIdentifier");
                writer.WriteValue(request.GroupingIdentifier);
            }
        }

        private void WriteExpirationKeys(JsonWriter writer, PassGeneratorRequest request)
        {
            if (request.ExpirationDate.HasValue)
            {
                writer.WritePropertyName("expirationDate");
                writer.WriteValue(request.ExpirationDate.Value.ToString("yyyy-MM-ddTHH:mm:sszzz"));
            }

            if (request.Voided.HasValue)
            {
                writer.WritePropertyName("voided");
                writer.WriteValue(request.Voided.Value);
            }
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
                    writer.WritePropertyName("storeCard");
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

        private string ConvertColor(string colour)
        {
            if (colour != null && colour.Length > 0 && colour.Substring(0, 1) == "#")
            {
                Color c = ColorTranslator.FromHtml(colour);
                return string.Format("rgb({0},{1},{2})", c.R, c.G, c.B);
            }
            else
            {
                return colour;
            }
        }

        #endregion

        public bool IsValid { get { return true; } }
    }
}
