using Passbook.Generator.Exceptions;
using Passbook.Generator.Fields;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Passbook.Generator;

public class PassGeneratorRequest
{
    public PassGeneratorRequest()
    {
        SemanticTags = [];
        HeaderFields = [];
        PrimaryFields = [];
        SecondaryFields = [];
        AuxiliaryFields = [];
        BackFields = [];
        Images = [];
        RelevantLocations = [];
        RelevantBeacons = [];
        AssociatedStoreIdentifiers = [];
        Localizations = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);
        ImageLocalizations = new Dictionary<string, Dictionary<PassbookImage, byte[]>>(StringComparer.OrdinalIgnoreCase);
        Barcodes = [];
        UserInfo = new Dictionary<string, object>();
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

    #endregion

    #region Images Files

    /// <summary>
    /// When using in memory, the binary of each image is put here.
    /// </summary>
    public Dictionary<PassbookImage, byte[]> Images { get; set; }

    #endregion

    #region Companion App Keys

    #endregion

    #region Expiration Keys

    public DateTimeOffset? ExpirationDate { get; set; }

    public bool? Voided { get; set; }

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
    /// Optional. The semantic tags to add to the pass. Read more about them here https://developer.apple.com/documentation/walletpasses/semantictags
    /// </summary>
    public SemanticTags SemanticTags { get; }

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

    #region Certificates

    /// <summary>
    /// A byte array containing the PassKit certificate
    /// </summary>
    public X509Certificate2 PassbookCertificate { get; set; }

    /// <summary>
    /// A byte array containing the Apple WWDRCA X509 certificate
    /// </summary>
    public X509Certificate2 AppleWWDRCACertificate { get; set; }

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

    public List<long> AssociatedStoreIdentifiers { get; set; }

    public string AppLaunchURL { get; set; }

    #endregion

    #region Barcodes

    public List<Barcode> Barcodes { get; private set; }

    #endregion

    #region User Info Keys

    public IDictionary<string, object> UserInfo { get; set; }

    #endregion

    #region Localization
    public Dictionary<string, Dictionary<string, string>> Localizations { get; set; }

    public Dictionary<string, Dictionary<PassbookImage, byte[]>> ImageLocalizations { get; set; }

    #endregion

    #region NFC

    public Nfc Nfc { get; set; }

    #endregion

    #region Helpers and Serialization

    public void AddHeaderField(Field field)
    {
        EnsureFieldKeyIsUnique(field.Key);
        HeaderFields.Add(field);
    }

    public void AddPrimaryField(Field field)
    {
        EnsureFieldKeyIsUnique(field.Key);
        PrimaryFields.Add(field);
    }

    public void AddSecondaryField(Field field)
    {
        EnsureFieldKeyIsUnique(field.Key);
        SecondaryFields.Add(field);
    }

    public void AddAuxiliaryField(Field field)
    {
        EnsureFieldKeyIsUnique(field.Key);
        AuxiliaryFields.Add(field);
    }

    public void AddBackField(Field field)
    {
        EnsureFieldKeyIsUnique(field.Key);
        BackFields.Add(field);
    }

    private void EnsureFieldKeyIsUnique(string key)
    {
        if (HeaderFields.Any(x => x.Key == key) ||
            PrimaryFields.Any(x => x.Key == key) ||
            SecondaryFields.Any(x => x.Key == key) ||
            AuxiliaryFields.Any(x => x.Key == key) ||
            BackFields.Any(x => x.Key == key))
        {
            throw new DuplicateFieldKeyException(key);
        }
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

    public void AddImageLocalization(string languageCode, PassbookImage key, byte[] value)
    {
        if (!ImageLocalizations.TryGetValue(languageCode, out Dictionary<PassbookImage, byte[]> values))
        {
            values = new Dictionary<PassbookImage, byte[]>();
            ImageLocalizations.Add(languageCode, values);
        }

        values[key] = value;
    }

    public virtual void PopulateFields()
    {
        // NO OP.
    }

    public void Write(Utf8JsonWriter writer)
    {
        PopulateFields();

        writer.WriteStartObject();

        Trace.TraceInformation("Writing semantics..");
        WriteSemantics(writer);
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

        if (Nfc != null)
        {
            Trace.TraceInformation("Writing NFC fields");
            WriteNfcKeys(writer);
        }

        Trace.TraceInformation("Opening style section..");
        OpenStyleSpecificKey(writer);

        Trace.TraceInformation("Writing header fields");
        WriteSection(writer, "headerFields", HeaderFields);
        Trace.TraceInformation("Writing primary fields");
        WriteSection(writer, "primaryFields", PrimaryFields);
        Trace.TraceInformation("Writing secondary fields");
        WriteSection(writer, "secondaryFields", SecondaryFields);
        Trace.TraceInformation("Writing auxiliary fields");
        WriteSection(writer, "auxiliaryFields", AuxiliaryFields);
        Trace.TraceInformation("Writing back fields");
        WriteSection(writer, "backFields", BackFields);

        if (Style == PassStyle.BoardingPass)
        {
            writer.WritePropertyName("transitType");
            writer.WriteStringValue(TransitType.ToString());
        }

        Trace.TraceInformation("Closing style section..");
        CloseStyleSpecificKey(writer);

        WriteBarcode(writer);
        WriteUrls(writer);

        writer.WriteEndObject();
    }

    private void WriteRelevanceKeys(Utf8JsonWriter writer)
    {
        if (RelevantDate.HasValue)
        {
            writer.WritePropertyName("relevantDate");
            writer.WriteStringValue(RelevantDate.Value.ToString("yyyy-MM-ddTHH:mm:ssK"));
        }

        if (MaxDistance.HasValue)
        {
            writer.WritePropertyName("maxDistance");
            writer.WriteStringValue(MaxDistance.Value.ToString());
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

    private void WriteUrls(Utf8JsonWriter writer)
    {
        if (!string.IsNullOrEmpty(AuthenticationToken))
        {
            writer.WritePropertyName("authenticationToken");
            writer.WriteStringValue(AuthenticationToken);
            writer.WritePropertyName("webServiceURL");
            writer.WriteStringValue(WebServiceUrl);
        }
    }

    private void WriteBarcode(Utf8JsonWriter writer)
    {
        if (Barcode != null)
        {
            writer.WritePropertyName("barcode");
            Barcode.Write(writer);
        }
    }

    private void WriteBarcodes(Utf8JsonWriter writer)
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

    private void WriteSemantics(Utf8JsonWriter writer)
    {
        SemanticTags.Write(writer);
    }

    private void WriteStandardKeys(Utf8JsonWriter writer)
    {
        writer.WritePropertyName("passTypeIdentifier");
        writer.WriteStringValue(PassTypeIdentifier);

        writer.WritePropertyName("formatVersion");
        writer.WriteNumberValue(FormatVersion);

        writer.WritePropertyName("serialNumber");
        writer.WriteStringValue(SerialNumber);

        writer.WritePropertyName("description");
        writer.WriteStringValue(Description);

        writer.WritePropertyName("organizationName");
        writer.WriteStringValue(OrganizationName);

        writer.WritePropertyName("teamIdentifier");
        writer.WriteStringValue(TeamIdentifier);

        writer.WritePropertyName("sharingProhibited");
        writer.WriteBooleanValue(SharingProhibited);

        if (!string.IsNullOrEmpty(LogoText))
        {
            writer.WritePropertyName("logoText");
            writer.WriteStringValue(LogoText);
        }

        if (AssociatedStoreIdentifiers.Count > 0)
        {
            writer.WritePropertyName("associatedStoreIdentifiers");
            writer.WriteStartArray();

            foreach (var storeIdentifier in AssociatedStoreIdentifiers)
            {
                writer.WriteNumberValue(storeIdentifier);
            }

            writer.WriteEndArray();
        }

        if (!string.IsNullOrEmpty(AppLaunchURL))
        {
            writer.WritePropertyName("appLaunchURL");
            writer.WriteStringValue(AppLaunchURL);
        }
    }

    private void WriteUserInfo(Utf8JsonWriter writer)
    {
        if (UserInfo != null)
        {
            writer.WritePropertyName("userInfo");
            string json = JsonSerializer.Serialize(UserInfo);
            writer.WriteRawValue(json);
        }
    }

    private void WriteAppearanceKeys(Utf8JsonWriter writer)
    {
        if (!string.IsNullOrEmpty(ForegroundColor))
        {
            writer.WritePropertyName("foregroundColor");
            writer.WriteStringValue(ConvertColor(ForegroundColor));
        }

        if (!string.IsNullOrEmpty(BackgroundColor))
        {
            writer.WritePropertyName("backgroundColor");
            writer.WriteStringValue(ConvertColor(BackgroundColor));
        }

        if (!string.IsNullOrEmpty(LabelColor))
        {
            writer.WritePropertyName("labelColor");
            writer.WriteStringValue(ConvertColor(LabelColor));
        }

        if (SuppressStripShine.HasValue)
        {
            writer.WritePropertyName("suppressStripShine");
            writer.WriteBooleanValue(SuppressStripShine.Value);
        }

        if (!string.IsNullOrEmpty(GroupingIdentifier))
        {
            writer.WritePropertyName("groupingIdentifier");
            writer.WriteStringValue(GroupingIdentifier);
        }
    }

    private void WriteExpirationKeys(Utf8JsonWriter writer)
    {
        if (ExpirationDate.HasValue)
        {
            writer.WritePropertyName("expirationDate");
            writer.WriteStringValue(ExpirationDate.Value.ToString("yyyy-MM-ddTHH:mm:ssK"));
        }

        if (Voided.HasValue)
        {
            writer.WritePropertyName("voided");
            writer.WriteBooleanValue(Voided.Value);
        }
    }

    private void OpenStyleSpecificKey(Utf8JsonWriter writer)
    {
        string key = Style.ToString();

        writer.WritePropertyName(char.ToLowerInvariant(key[0]) + key.Substring(1));
        writer.WriteStartObject();
    }

    private static void CloseStyleSpecificKey(Utf8JsonWriter writer)
    {
        writer.WriteEndObject();
    }

    private static void WriteSection(Utf8JsonWriter writer, string sectionName, List<Field> fields)
    {
        writer.WritePropertyName(sectionName);
        writer.WriteStartArray();

        foreach (var field in fields)
        {
            field.Write(writer);
        }

        writer.WriteEndArray();
    }

    private void WriteNfcKeys(Utf8JsonWriter writer)
    {
        if (!string.IsNullOrEmpty(Nfc.Message))
        {
            writer.WritePropertyName("nfc");
            writer.WriteStartObject();
            writer.WritePropertyName("message");
            writer.WriteStringValue(Nfc.Message);

            if (!string.IsNullOrEmpty(Nfc.EncryptionPublicKey))
            {
                writer.WritePropertyName("encryptionPublicKey");
                writer.WriteStringValue(Nfc.EncryptionPublicKey);
            }

            writer.WriteEndObject();
        }
    }

    private static string ConvertColor(string color)
    {
        if (!string.IsNullOrEmpty(color) && color.Substring(0, 1) == "#")
        {
            int r, g, b;

            if (color.Length == 3)
            {
                r = int.Parse(color.Substring(1, 1), NumberStyles.HexNumber);
                g = int.Parse(color.Substring(2, 1), NumberStyles.HexNumber);
                b = int.Parse(color.Substring(3, 1), NumberStyles.HexNumber);
            }
            else if (color.Length >= 6)
            {
                r = int.Parse(color.Substring(1, 2), NumberStyles.HexNumber);
                g = int.Parse(color.Substring(3, 2), NumberStyles.HexNumber);
                b = int.Parse(color.Substring(5, 2), NumberStyles.HexNumber);
            }
            else
            {
                throw new ArgumentException("use #rgb or #rrggbb for color values", color);
            }

            return $"rgb({r},{g},{b})";
        }
        else
        {
            return color;
        }
    }

    #endregion
}
