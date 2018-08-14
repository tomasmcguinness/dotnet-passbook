using System;
using System.Linq;
using Newtonsoft.Json;
using Passbook.Generator.Fields;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Drawing;
using System.Diagnostics;
using Passbook.Generator.Configuration;
using System.IO;
using System.Configuration;

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
            this.Localizations = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);
            this.Barcodes = new List<Barcode>();
            this.UserInfo = null;
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
        /// <summary>
        /// Disables sharing of the pass.
        /// </summary>
        public bool SharingProhibited { get; set; }

        #region Images Files

        /// <summary>
        /// When using in memory, the binary of each image is put here.
        /// </summary>
        public Dictionary<PassbookImage, byte[]> Images { get; set; }

        #endregion
        #endregion

        #region Companion App Keys

        #endregion

        #region Expiration Keys

        public DateTimeOffset? ExpirationDate { get; set; }

        public Boolean? Voided { get; set; }

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
        public Barcode Barcode { get; private set; }

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
        /// Optional. Date and time when the pass becomes relevant. For example, the start time of a movie.
        /// </summary>
        public DateTimeOffset? RelevantDate { get; set; }

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

        #region Barcodes

        public List<Barcode> Barcodes { get; private set; }

        #endregion

        #region User Info Keys

        public Object UserInfo { get; set; }

        #endregion

        #region Localization
        public Dictionary<string, Dictionary<string, string>> Localizations { get; set; }
        #endregion

        #region Helpers

        public void AddHeaderField(Field field)
        {
            HeaderFields.Add(field);
        }

        public void AddPrimaryField(Field field)
        {
            PrimaryFields.Add(field);
        }

        public void AddSecondaryField(Field field)
        {
            SecondaryFields.Add(field);
        }

        public void AddAuxiliaryField(Field field)
        {
            AuxiliaryFields.Add(field);
        }

        public void AddBackField(Field field)
        {
            BackFields.Add(field);
        }

        public void AddBarcode(BarcodeType type, string message, string encoding, string alternateText)
        {
            Barcodes.Add(new Barcode(type, message, encoding, alternateText));
        }

        public void AddBarcode(BarcodeType type, string message, string encoding)
        {
            Barcodes.Add(new Barcode(type, message, encoding));
        }

        public void SetBarcode(BarcodeType type, string message, string encoding, string alternateText = null)
        {
            Barcode = new Barcode(type, message, encoding, alternateText);
        }

        public void AddLocation(double latitude, double longitude)
        {
            AddLocation(latitude, longitude, null);
        }

        public void AddLocation(double latitude, double longitude, string relevantText)
        {
            RelevantLocations.Add(new RelevantLocation() { Latitude = latitude, Longitude = longitude, RelevantText = relevantText });
        }

        public void AddBeacon(string proximityUUID, string relevantText)
        {
            RelevantBeacons.Add(new RelevantBeacon() { ProximityUUID = proximityUUID, RelevantText = relevantText });
        }

        public void AddBeacon(string proximityUUID, string relevantText, int major)
        {
            RelevantBeacons.Add(new RelevantBeacon() { ProximityUUID = proximityUUID, RelevantText = relevantText, Major = major });
        }

        public void AddBeacon(string proximityUUID, string relevantText, int major, int minor)
        {
            RelevantBeacons.Add(new RelevantBeacon() { ProximityUUID = proximityUUID, RelevantText = relevantText, Major = major, Minor = minor });
        }

        public void AddLocalization(string languageCode, string key, string value)
        {
            Dictionary<string, string> values;

            if (!Localizations.TryGetValue(languageCode, out values))
            {
                values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                Localizations.Add(languageCode, values);
            }

            values[key] = value;
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
            WriteStandardKeys(writer);
            Trace.TraceInformation("Writing user information..");
            WriteUserInfo(writer);
            Trace.TraceInformation("Writing relevance keys..");
            WriteRelevanceKeys(writer);
            Trace.TraceInformation("Writing appearance keys..");
            WriteAppearanceKeys(writer);
            Trace.TraceInformation("Writing expiration keys..");
            WriteExpirationKeys(writer);
            Trace.TraceInformation("Writing barcode keys..");
            WriteBarcodes(writer);

            Trace.TraceInformation("Opening style section..");
            OpenStyleSpecificKey(writer);

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

            WriteBarcode(writer);
            WriteUrls(writer);

            writer.WriteEndObject();
        }

        private void WriteRelevanceKeys(JsonWriter writer)
        {
            if (RelevantDate.HasValue)
            {
                writer.WritePropertyName("relevantDate");
                writer.WriteValue(RelevantDate.Value.ToString("yyyy-MM-ddTHH:mm:ssK"));
            }

            if (MaxDistance.HasValue)
            {
                writer.WritePropertyName("maxDistance");
                writer.WriteValue(MaxDistance.Value.ToString());
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

        private void WriteUrls(JsonWriter writer)
        {
            if (!string.IsNullOrEmpty(AuthenticationToken))
            {
                writer.WritePropertyName("authenticationToken");
                writer.WriteValue(AuthenticationToken);
                writer.WritePropertyName("webServiceURL");
                writer.WriteValue(WebServiceUrl);
            }
        }

        private void WriteBarcode(JsonWriter writer)
        {
            if (Barcode != null)
            {
                writer.WritePropertyName("barcode");
                Barcode.Write(writer);
            }
        }

        private void WriteBarcodes(JsonWriter writer)
        {
            if (Barcodes.Count > 0)
            {
                writer.WritePropertyName("barcodes");
                writer.WriteStartArray();

                foreach (var barcode in Barcodes)
                {
                    barcode.Write(writer);
                }

                writer.WriteEndArray();
            }
        }

        private void WriteStandardKeys(JsonWriter writer)
        {
            writer.WritePropertyName("passTypeIdentifier");
            writer.WriteValue(PassTypeIdentifier);

            writer.WritePropertyName("formatVersion");
            writer.WriteValue(FormatVersion);

            writer.WritePropertyName("serialNumber");
            writer.WriteValue(SerialNumber);

            writer.WritePropertyName("description");
            writer.WriteValue(Description);

            writer.WritePropertyName("organizationName");
            writer.WriteValue(OrganizationName);

            writer.WritePropertyName("teamIdentifier");
            writer.WriteValue(TeamIdentifier);

            writer.WritePropertyName("sharingProhibited");
            writer.WriteValue(SharingProhibited);

            if (!String.IsNullOrEmpty(LogoText))
            {
                writer.WritePropertyName("logoText");
                writer.WriteValue(LogoText);
            }

            if (AssociatedStoreIdentifiers.Count > 0)
            {
                writer.WritePropertyName("associatedStoreIdentifiers");
                writer.WriteStartArray();

                foreach (int storeIdentifier in AssociatedStoreIdentifiers)
                {
                    writer.WriteValue(storeIdentifier);
                }

                writer.WriteEndArray();
            }

            if(!string.IsNullOrEmpty(AppLaunchURL))
            {
                writer.WritePropertyName("appLaunchURL");
                writer.WriteValue(AppLaunchURL);
            }
        }

        private void WriteUserInfo(JsonWriter writer)
        {
            if (UserInfo != null)
            {
                writer.WritePropertyName("userInfo");
                writer.WriteRawValue(JsonConvert.SerializeObject(UserInfo));
            }
        }

        private void WriteAppearanceKeys(JsonWriter writer)
        {
            if (!String.IsNullOrEmpty(ForegroundColor))
            {
                writer.WritePropertyName("foregroundColor");
                writer.WriteValue(ConvertColor(ForegroundColor));
            }

            if (!String.IsNullOrEmpty(BackgroundColor))
            {
                writer.WritePropertyName("backgroundColor");
                writer.WriteValue(ConvertColor(BackgroundColor));
            }

            if (!String.IsNullOrEmpty(LabelColor))
            {
                writer.WritePropertyName("labelColor");
                writer.WriteValue(ConvertColor(LabelColor));
            }

            if (SuppressStripShine.HasValue)
            {
                writer.WritePropertyName("suppressStripShine");
                writer.WriteValue(SuppressStripShine.Value);
            }

            if (!String.IsNullOrEmpty(GroupingIdentifier))
            {
                writer.WritePropertyName("groupingIdentifier");
                writer.WriteValue(GroupingIdentifier);
            }
        }

        private void WriteExpirationKeys(JsonWriter writer)
        {
            if (ExpirationDate.HasValue)
            {
                writer.WritePropertyName("expirationDate");
                writer.WriteValue(ExpirationDate.Value.ToString("yyyy-MM-ddTHH:mm:ssK"));
            }

            if (Voided.HasValue)
            {
                writer.WritePropertyName("voided");
                writer.WriteValue(Voided.Value);
            }
        }

        private void OpenStyleSpecificKey(JsonWriter writer)
        {
            String key = Style.ToString();

            writer.WritePropertyName(Char.ToLowerInvariant(key[0]) + key.Substring(1));
            writer.WriteStartObject();
        }

        private static void CloseStyleSpecificKey(JsonWriter writer)
        {
            writer.WriteEndObject();
        }

        private static void WriteSection(JsonWriter writer, string sectionName, List<Field> fields)
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

        public bool IsValid { get { return true; } }

        private static string ConvertColor(string color)
        {
            if (!String.IsNullOrEmpty(color) && color.Substring(0, 1) == "#")
            {
                Color c = ColorTranslator.FromHtml(color);
                return string.Format("rgb({0},{1},{2})", c.R, c.G, c.B);
            }
            else
            {
                return color;
            }
        }

        public void LoadTemplate(string template, TemplateModel parameters)
        {
            PassbookGeneratorSection section = ConfigurationManager.GetSection("passbookGenerator") as PassbookGeneratorSection;

            if (section == null)
                throw new System.Configuration.ConfigurationErrorsException("\"passbookGenerator\" section could not be loaded.");

            String path = TemplateModel.MapPath(section.AppleWWDRCACertificate);
            if (File.Exists(path))
                this.AppleWWDRCACertificate = File.ReadAllBytes(path);

            TemplateElement templateConfig = section
                .Templates
                .OfType<TemplateElement>()
                .FirstOrDefault(t => String.Equals(t.Name, template, StringComparison.OrdinalIgnoreCase));

            if (templateConfig == null)
                throw new System.Configuration.ConfigurationErrorsException(String.Format("Configuration for template \"{0}\" could not be loaded.", template));

            this.Style = templateConfig.PassStyle;

            if (this.Style == PassStyle.BoardingPass)
                this.TransitType = templateConfig.TransitType;

            // Certificates
            this.CertificatePassword = templateConfig.CertificatePassword;
            this.CertThumbprint = templateConfig.CertificateThumbprint;

            path = TemplateModel.MapPath(templateConfig.Certificate);
            if (File.Exists(path))
                this.Certificate = File.ReadAllBytes(path);

            if (String.IsNullOrEmpty(this.CertThumbprint) && this.Certificate == null)
            {
                throw new System.Configuration.ConfigurationErrorsException("Either Certificate or CertificateThumbprint is not configured correctly.");
            }

            // Standard Keys
            this.Description = templateConfig.Description.Value;
            this.OrganizationName = templateConfig.OrganizationName.Value;
            this.PassTypeIdentifier = templateConfig.PassTypeIdentifier.Value;
            this.TeamIdentifier = templateConfig.TeamIdentifier.Value;

            // Associated App Keys
            if (templateConfig.AppLaunchURL != null && !String.IsNullOrEmpty(templateConfig.AppLaunchURL.Value))
                this.AppLaunchURL = templateConfig.AppLaunchURL.Value;

            this.AssociatedStoreIdentifiers.AddRange(templateConfig.AssociatedStoreIdentifiers.OfType<ConfigurationProperty<int>>().Select(s => s.Value));

            // Visual Appearance Keys
            this.BackgroundColor = templateConfig.BackgroundColor.Value;
            this.ForegroundColor = templateConfig.ForegroundColor.Value;
            this.GroupingIdentifier = templateConfig.GroupingIdentifier.Value;
            this.LabelColor = templateConfig.LabelColor.Value;
            this.LogoText = templateConfig.LogoText.Value;
            this.SuppressStripShine = templateConfig.SuppressStripShine.Value;

            // Web Service Keys
            this.AuthenticationToken = templateConfig.AuthenticationToken.Value;
            this.WebServiceUrl = templateConfig.WebServiceURL.Value;

            // Fields
            this.AuxiliaryFields.AddRange(TemplateFields(templateConfig.AuxiliaryFields, parameters));
            this.BackFields.AddRange(TemplateFields(templateConfig.BackFields, parameters));
            this.HeaderFields.AddRange(TemplateFields(templateConfig.HeaderFields, parameters));
            this.PrimaryFields.AddRange(TemplateFields(templateConfig.PrimaryFields, parameters));
            this.SecondaryFields.AddRange(TemplateFields(templateConfig.SecondaryFields, parameters));

            // Template Images
            foreach (ImageElement image in templateConfig.Images)
            {
                String imagePath = TemplateModel.MapPath(image.FileName);
                if (File.Exists(imagePath))
                    this.Images[image.Type] = File.ReadAllBytes(imagePath);
            }

            // Model Images (Overwriting template images)
            foreach (KeyValuePair<PassbookImage, byte[]> image in parameters.GetImages())
            {
                this.Images[image.Key] = image.Value;
            }

            // Localization
            foreach (LanguageElement localization in templateConfig.Localizations)
            {
                Dictionary<string, string> values;

                if (!Localizations.TryGetValue(localization.Code, out values))
                {
                    values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    Localizations.Add(localization.Code, values);
                }

                foreach (LocalizedEntry entry in localization.Localizations)
                {
                    values[entry.Key] = entry.Value;
                }
            }
        }

        private static IEnumerable<Field> TemplateFields(FieldCollection templateFields, TemplateModel model)
        {
            foreach (FieldElement fieldElement in templateFields)
            {
                String key = fieldElement.Key;
                Field field = null;

                switch (fieldElement.Type)
                {
                    case FieldType.Standard:
                        StandardField standardField = new StandardField();
                        standardField.Value = model.GetField(key, FieldAttribute.Value, fieldElement.Value.Value);

                        field = standardField;
                        break;
                    case FieldType.Date:
                        DateField dateField = new DateField();

                        if (fieldElement.DateStyle != FieldDateTimeStyle.Unspecified)
                            dateField.DateStyle = fieldElement.DateStyle;

                        if (fieldElement.TimeStyle != FieldDateTimeStyle.Unspecified)
                            dateField.TimeStyle = fieldElement.TimeStyle;

                        if (fieldElement.IgnoresTimeZone.HasValue)
                            dateField.IgnoresTimeZone = fieldElement.IgnoresTimeZone.Value;

                        if (fieldElement.IsRelative.HasValue)
                            dateField.IsRelative = fieldElement.IsRelative.Value;

                        DateTime dateValue;

                        if (!DateTime.TryParse(fieldElement.Value.Value, out dateValue))
                            dateValue = DateTime.MinValue;

                        dateField.Value = model.GetField<DateTime>(key, FieldAttribute.Value, dateValue);

                        field = dateField;
                        break;
                    case FieldType.Number:
                        NumberField numberField = new NumberField();

                        if (fieldElement.NumberStyle != FieldNumberStyle.Unspecified)
                            numberField.NumberStyle = fieldElement.NumberStyle;

                        numberField.CurrencyCode = model.GetField(key, FieldAttribute.CurrencyCode, fieldElement.CurrencyCode.Value);

                        Decimal decimalValue;

                        if (!Decimal.TryParse(fieldElement.Value.Value, out decimalValue))
                            decimalValue = 0;

                        numberField.Value = model.GetField<Decimal>(key, FieldAttribute.Value, decimalValue);

                        field = numberField;
                        break;
                }

                field.Key = fieldElement.Key;

                field.DataDetectorTypes = fieldElement.DataDetectorTypes;

                if (field.TextAlignment != FieldTextAlignment.Unspecified)
                    field.TextAlignment = fieldElement.TextAlignment;

                field.Label = model.GetField(key, FieldAttribute.Label, fieldElement.Label.Value);
                field.AttributedValue = model.GetField(key, FieldAttribute.AttributedValue, fieldElement.AttributedValue.Value);
                field.ChangeMessage = model.GetField(key, FieldAttribute.ChangeMessage, fieldElement.ChangeMessage.Value);

                yield return field;
            }
        }
    }
}
