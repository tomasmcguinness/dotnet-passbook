using Passbook.Generator.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Passbook.Generator;

public class PassGenerator : IPassGenerator
{
    private byte[] passFile = null;
    private byte[] signatureFile = null;
    private byte[] manifestFile = null;
    private Dictionary<string, Dictionary<string, byte[]>> localizationFiles = null;
    private byte[] pkPassFile = null;
    private byte[] pkPassBundle = null;
    private const string passTypePrefix = "Pass Type ID: ";

    /// <exception cref="ArgumentNullException"></exception>
    public byte[] Generate(PassGeneratorRequest generatorRequest)
    {
        if (generatorRequest == null)
        {
            throw new ArgumentNullException(nameof(generatorRequest), "You must pass an instance of PassGeneratorRequest");
        }

        CreatePackage(generatorRequest);
        ZipPackage(generatorRequest);

        return pkPassFile;
    }

    /// <exception cref="System.ArgumentNullException">
    /// <exception cref="System.ArgumentException">
    public byte[] Generate(IReadOnlyList<PassGeneratorRequest> generatorRequests)
    {
        if (generatorRequests == null)
        {
            throw new ArgumentNullException(nameof(generatorRequests), "You must pass an instance of IReadOnlyList containing at least one PassGeneratorRequest");
        }

        if (!generatorRequests.Any())
        {
            throw new ArgumentException(nameof(generatorRequests), "The IReadOnlyList must contain at least one PassGeneratorRequest");
        }

        ZipBundle(generatorRequests);

        return pkPassBundle;
    }

    private void ZipBundle(IEnumerable<PassGeneratorRequest> generatorRequests)
    {
        using (MemoryStream zipToOpen = new MemoryStream())
        {
            using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Create, true))
            {
                int i = 0;

                foreach (PassGeneratorRequest generatorRequest in generatorRequests)
                {
                    ZipArchiveEntry zipEntry = archive.CreateEntry($"pass{i++}.pkpass");

                    CreatePackage(generatorRequest);
                    ZipPackage(generatorRequest);

                    using (MemoryStream originalFileStream = new MemoryStream(pkPassFile))
                    {
                        using (Stream zipEntryStream = zipEntry.Open())
                        {
                            originalFileStream.CopyTo(zipEntryStream);
                        }
                    }
                }
            }

            pkPassBundle = zipToOpen.ToArray();

            zipToOpen.Flush();
        }
    }

    private void ZipPackage(PassGeneratorRequest request)
    {
        using (MemoryStream zipToOpen = new MemoryStream())
        {
            using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update, true))
            {
                foreach (KeyValuePair<PassbookImage, byte[]> image in request.Images)
                {
                    ZipArchiveEntry imageEntry = archive.CreateEntry(image.Key.ToFilename());

                    using (BinaryWriter writer = new BinaryWriter(imageEntry.Open()))
                    {
                        writer.Write(image.Value);
                        writer.Flush();
                    }
                }

                foreach (KeyValuePair<string, Dictionary<string, byte[]>> localization in localizationFiles)
                {
                    foreach (KeyValuePair<string, byte[]> localizationFile in localization.Value)
                    {
                        ZipArchiveEntry localizationEntry = archive.CreateEntry(string.Format("{0}.lproj/{1}", localization.Key.ToLower(), localizationFile.Key.ToLower()));
                        
                        using (BinaryWriter writer = new BinaryWriter(localizationEntry.Open()))
                        {
                            writer.Write(localizationFile.Value);
                            writer.Flush();
                        }
                    }
                }

                ZipArchiveEntry PassJSONEntry = archive.CreateEntry(@"pass.json");
                using (BinaryWriter writer = new BinaryWriter(PassJSONEntry.Open()))
                {
                    writer.Write(passFile);
                    writer.Flush();
                }

                ZipArchiveEntry ManifestJSONEntry = archive.CreateEntry(@"manifest.json");
                using (BinaryWriter writer = new BinaryWriter(ManifestJSONEntry.Open()))
                {
                    writer.Write(manifestFile);
                    writer.Flush();
                }

                ZipArchiveEntry SignatureEntry = archive.CreateEntry(@"signature");
                using (BinaryWriter writer = new BinaryWriter(SignatureEntry.Open()))
                {
                    writer.Write(signatureFile);
                    writer.Flush();
                }
            }

            pkPassFile = zipToOpen.ToArray();
            zipToOpen.Flush();
        }
    }

    private void CreatePackage(PassGeneratorRequest request)
    {
        ValidateCertificates(request);
        CreatePassFile(request);
        GenerateLocalizationFiles(request);
        GenerateManifestFile(request);
    }

    private void ValidateCertificates(PassGeneratorRequest request)
    {
        if (request.PassbookCertificate == null)
        {
            throw new FileNotFoundException("Certificate could not be found. Please ensure the thumbprint and cert location values are correct.");
        }

        if (request.AppleWWDRCACertificate == null)
        {
            throw new FileNotFoundException("Apple Certificate could not be found. Please download it from http://www.apple.com/certificateauthority/ and install it into your PERSONAL or LOCAL MACHINE certificate store.");
        }

        string passTypeIdentifier = request.PassbookCertificate.GetNameInfo(X509NameType.SimpleName, false);

        if (passTypeIdentifier.StartsWith(passTypePrefix, StringComparison.OrdinalIgnoreCase))
        {
            passTypeIdentifier = passTypeIdentifier.Substring(passTypePrefix.Length);

            if (!string.IsNullOrEmpty(passTypeIdentifier) && !string.Equals(request.PassTypeIdentifier, passTypeIdentifier, StringComparison.Ordinal))
            {
                if (!string.IsNullOrEmpty(request.PassTypeIdentifier))
                {
                    Trace.TraceWarning("Configured passTypeIdentifier {0} does not match pass certificate {1}, correcting.", request.PassTypeIdentifier, passTypeIdentifier);
                }

                request.PassTypeIdentifier = passTypeIdentifier;
            }
        }

        Dictionary<string, string> nameParts =
        Regex.Matches(request.PassbookCertificate.SubjectName.Name, @"(?<key>[^=,\s]+)\=*(?<value>("".+""|[^,])+)")
          .Cast<Match>()
          .ToDictionary(
              m => m.Groups["key"].Value,
              m => m.Groups["value"].Value);

        string teamIdentifier;

        if (nameParts.TryGetValue("OU", out teamIdentifier))
        {
            if (!string.IsNullOrEmpty(teamIdentifier) && !string.Equals(request.TeamIdentifier, teamIdentifier, StringComparison.Ordinal))
            {
                if (!string.IsNullOrEmpty(request.TeamIdentifier))
                {
                    Trace.TraceWarning("Configured teamidentifier {0} does not match pass certificate {1}, correcting.", request.TeamIdentifier, teamIdentifier);
                }

                request.TeamIdentifier = teamIdentifier;
            }
        }
    }

    private void CreatePassFile(PassGeneratorRequest request)
    {
        using var ms = new MemoryStream();
        var options = new JsonWriterOptions { Indented = true };
        var writer = new Utf8JsonWriter(ms, options);

        Trace.TraceInformation("Writing JSON...");
        request.Write(writer);
        writer.Flush();
        passFile = ms.ToArray();
    }

    private void GenerateLocalizationFiles(PassGeneratorRequest request)
    {
        localizationFiles = new Dictionary<string, Dictionary<string, byte[]>>(StringComparer.OrdinalIgnoreCase);

        foreach (KeyValuePair<string, Dictionary<string, string>> localization in request.Localizations)
        {
            using var ms = new MemoryStream();
            using var sr = new StreamWriter(ms, Encoding.UTF8);
            foreach (KeyValuePair<string, string> value in localization.Value)
            {
                sr.WriteLine("\"{0}\" = \"{1}\";\n", value.Key, value.Value);
            }

            sr.Flush();

            var files = new Dictionary<string, byte[]>
            {
                { "pass.strings", ms.ToArray() }
            };

            localizationFiles.Add(localization.Key, files);
        }

        foreach (KeyValuePair<string, Dictionary<PassbookImage, byte[]>> localization in request.ImageLocalizations)
        {
            if (!localizationFiles.TryGetValue(localization.Key, out var files))
            {
                files = new Dictionary<string, byte[]>();
                localizationFiles[localization.Key] = files;
            }

            foreach (KeyValuePair<PassbookImage, byte[]> imageLocalization in localization.Value)
            {
                files.Add(imageLocalization.Key.ToFilename(), imageLocalization.Value);
            }
        }
    }


    private void GenerateManifestFile(PassGeneratorRequest request)
    {
        using var ms = new MemoryStream();
        var options = new JsonWriterOptions { Indented = true };
        var jsonWriter = new Utf8JsonWriter(ms, options);
        jsonWriter.WriteStartObject();

        string hash = null;

        hash = GetHashForBytes(passFile);
        jsonWriter.WritePropertyName(@"pass.json");
        jsonWriter.WriteStringValue(hash);

        foreach (KeyValuePair<PassbookImage, byte[]> image in request.Images)
        {
            try
            {
                hash = GetHashForBytes(image.Value);
                jsonWriter.WritePropertyName(image.Key.ToFilename());
                jsonWriter.WriteStringValue(hash);
            }
            catch (Exception exception)
            {
                throw new Exception($"Unexpect error writing image on manifest file. Image Key: \"{image.Key}\"", exception);
            }
        }

        foreach (KeyValuePair<string, Dictionary<string, byte[]>> localization in localizationFiles)
        {
            foreach (KeyValuePair<string, byte[]> localizationFile in localization.Value)
            {
                hash = GetHashForBytes(localizationFile.Value);
                jsonWriter.WritePropertyName(string.Format("{0}.lproj/{1}", localization.Key.ToLower(), localizationFile.Key.ToLower()));
                jsonWriter.WriteStringValue(hash);
            }
        }

        jsonWriter.WriteEndObject();
        jsonWriter.Flush();

        manifestFile = ms.ToArray();

        SignManifestFile(request);
    }

    private void SignManifestFile(PassGeneratorRequest request)
    {
        Trace.TraceInformation("Signing the manifest file...");

        try
        {
            ContentInfo contentInfo = new ContentInfo(manifestFile);

            SignedCms signing = new SignedCms(contentInfo, true);

            CmsSigner signer = new CmsSigner(SubjectIdentifierType.SubjectKeyIdentifier, request.PassbookCertificate)
            {
                IncludeOption = X509IncludeOption.None
            };

            Trace.TraceInformation("Fetching Apple Certificate for signing..");
            Trace.TraceInformation("Constructing the certificate chain..");
            signer.Certificates.Add(request.AppleWWDRCACertificate);
            signer.Certificates.Add(request.PassbookCertificate);

            signer.SignedAttributes.Add(new Pkcs9SigningTime());

            Trace.TraceInformation("Processing the signature..");
            signing.ComputeSignature(signer);

            signatureFile = signing.Encode();

            Trace.TraceInformation("The file has been successfully signed!");
        }
        catch (Exception exp)
        {
            Trace.TraceError("Failed to sign the manifest file: [{0}]", exp.Message);
            throw new ManifestSigningException("Failed to sign manifest", exp);
        }
    }

    private string GetHashForBytes(byte[] bytes)
    {
        using SHA1 hasher = SHA1.Create();
        return BitConverter.ToString(hasher.ComputeHash(bytes)).Replace("-", string.Empty).ToLower();
    }
}
