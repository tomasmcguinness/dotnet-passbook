using Newtonsoft.Json;
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
using System.Text.RegularExpressions;

namespace Passbook.Generator
{
    public class PassGenerator
    {
        private byte[] passFile = null;
        private byte[] signatureFile = null;
        private byte[] manifestFile = null;
        private Dictionary<string, byte[]> localizationFiles = null;
        private byte[] pkPassFile = null;
        private X509Certificate2 appleCert = null;
        private X509Certificate2 passCert = null;

        private const string APPLE_CERTIFICATE_THUMBPRINT = "FF6797793A3CD798DC5B2ABEF56F73EDC9F83A64";
        private const string passTypePrefix = "Pass Type ID: ";

        public byte[] Generate(PassGeneratorRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request", "You must pass an instance of PassGeneratorRequest");
            }

            CreatePackage(request);
            ZipPackage(request);

            return pkPassFile;
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

                    foreach (KeyValuePair<string, byte[]> localization in localizationFiles)
                    {
                        ZipArchiveEntry localizationEntry = archive.CreateEntry(string.Format("{0}.lproj/pass.strings", localization.Key.ToLower()));

                        using (BinaryWriter writer = new BinaryWriter(localizationEntry.Open()))
                        {
                            writer.Write(localization.Value);
                            writer.Flush();
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
            passCert = GetCertificate(request);

            if (passCert == null)
            {
                throw new FileNotFoundException("Certificate could not be found. Please ensure the thumbprint and cert location values are correct.");
            }

            appleCert = GetAppleCertificate(request);

            if (appleCert == null)
            {
                throw new FileNotFoundException("Apple Certificate could not be found. Please download it from http://www.apple.com/certificateauthority/ and install it into your PERSONAL or LOCAL MACHINE certificate store.");
            }

            string passTypeIdentifier = passCert.GetNameInfo(X509NameType.SimpleName, false);

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
            Regex.Matches(passCert.SubjectName.Name, @"(?<key>[^=,\s]+)\=*(?<value>("".+""|[^,])+)")
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
            using (MemoryStream ms = new MemoryStream())
            {
                using (StreamWriter sr = new StreamWriter(ms))
                {
                    using (JsonWriter writer = new JsonTextWriter(sr))
                    {
                        writer.Formatting = Formatting.Indented;

                        Trace.TraceInformation("Writing JSON...");
                        request.Write(writer);
                    }

                    passFile = ms.ToArray();
                }
            }
        }

        private void GenerateLocalizationFiles(PassGeneratorRequest request)
        {
            localizationFiles = new Dictionary<string, byte[]>(StringComparer.OrdinalIgnoreCase);

            foreach (KeyValuePair<string, Dictionary<string, string>> localization in request.Localizations)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (StreamWriter sr = new StreamWriter(ms, Encoding.UTF8))
                    {
                        foreach (KeyValuePair<string, string> value in localization.Value)
                        {
                            sr.WriteLine("\"{0}\" = \"{1}\";\n", value.Key, value.Value);
                        }

                        sr.Flush();
                        localizationFiles.Add(localization.Key, ms.ToArray());
                    }
                }
            }
        }

        private void GenerateManifestFile(PassGeneratorRequest request)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (StreamWriter sw = new StreamWriter(ms))
                {
                    using (JsonWriter jsonWriter = new JsonTextWriter(sw))
                    {
                        jsonWriter.Formatting = Formatting.Indented;
                        jsonWriter.WriteStartObject();

                        string hash = null;

                        hash = GetHashForBytes(passFile);
                        jsonWriter.WritePropertyName(@"pass.json");
                        jsonWriter.WriteValue(hash);

                        foreach (KeyValuePair<PassbookImage, byte[]> image in request.Images)
                        {
                            try
                            {
                                hash = GetHashForBytes(image.Value);
                                jsonWriter.WritePropertyName(image.Key.ToFilename());
                                jsonWriter.WriteValue(hash);
                            }
                            catch (Exception exception)
                            {
                                throw new Exception($"Unexpect error writing image on manifest file. Image Key: \"{image.Key}\"", exception);
                            }
                        }

                        foreach (KeyValuePair<string, byte[]> localization in localizationFiles)
                        {
                            hash = GetHashForBytes(localization.Value);
                            jsonWriter.WritePropertyName(string.Format("{0}.lproj/pass.strings", localization.Key.ToLower()));
                            jsonWriter.WriteValue(hash);
                        }
                    }

                    manifestFile = ms.ToArray();
                }

                SignManifestFile(request);
            }
        }

        private void SignManifestFile(PassGeneratorRequest request)
        {
            Trace.TraceInformation("Signing the manifest file...");

            try
            {
                ContentInfo contentInfo = new ContentInfo(manifestFile);

                SignedCms signing = new SignedCms(contentInfo, true);

                CmsSigner signer = new CmsSigner(SubjectIdentifierType.SubjectKeyIdentifier, passCert)
                {
                    IncludeOption = X509IncludeOption.None
                };

                Trace.TraceInformation("Fetching Apple Certificate for signing..");
                Trace.TraceInformation("Constructing the certificate chain..");
                signer.Certificates.Add(appleCert);
                signer.Certificates.Add(passCert);

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

        private X509Certificate2 GetAppleCertificate(PassGeneratorRequest request)
        {
            Trace.TraceInformation("Fetching Apple Certificate...");

            try
            {
                if (request.AppleWWDRCACertificate == null)
                {
                    return GetSpecifiedCertificateFromCertStore(APPLE_CERTIFICATE_THUMBPRINT, StoreName.CertificateAuthority, X509FindType.FindByThumbprint);
                }
                else
                {
                    return GetCertificateFromBytes(request.AppleWWDRCACertificate, null);
                }
            }
            catch (Exception exp)
            {
                Trace.TraceError("Failed to fetch Apple Certificate: [{0}]", exp.Message);
                throw;
            }
        }

        private static X509Certificate2 GetCertificate(PassGeneratorRequest request)
        {
            Trace.TraceInformation("Fetching Pass Certificate...");

            try
            {
                if (request.Certificate == null)
                {
                    if (string.IsNullOrEmpty(request.CertThumbprint))
                    {
                        return GetSpecifiedCertificateFromCertStore(request.PassTypeIdentifier, StoreName.My, X509FindType.FindBySubjectName);
                    }
                    else
                    {
                        return GetSpecifiedCertificateFromCertStore(request.CertThumbprint, StoreName.My, X509FindType.FindByThumbprint);
                    }
                }
                else
                {
                    return GetCertificateFromBytes(request.Certificate, request.CertificatePassword);
                }
            }
            catch (Exception exp)
            {
                Trace.TraceError("Failed to fetch Pass Certificate: [{0}]", exp.Message);
                throw;
            }
        }

        private static X509Certificate2 GetSpecifiedCertificateFromCertStore(string searchValue, StoreName storeName, X509FindType findType)
        {
            foreach (StoreLocation storeLocation in Enum.GetValues(typeof(StoreLocation)))
            {
                X509Store store = new X509Store(storeName, storeLocation);
                store.Open(OpenFlags.ReadOnly);

                X509Certificate2Collection certs = store.Certificates.Find(findType, searchValue, false);

                if (certs.Count > 0)
                {
                    Debug.WriteLine(certs[0].Thumbprint);
                    return certs[0];
                }
            }

            return null;
        }

        private static X509Certificate2 GetCertificateFromBytes(byte[] bytes, string password)
        {
            Trace.TraceInformation("Opening Certificate: [{0}] bytes with password [{1}]", bytes.Length, password);

            X509KeyStorageFlags flags = X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable;
            X509Certificate2 certificate = new X509Certificate2(bytes, password, flags);

            return certificate;
        }

        private string GetHashForBytes(byte[] bytes)
        {
            using (SHA1CryptoServiceProvider hasher = new SHA1CryptoServiceProvider())
            {
                return System.BitConverter.ToString(hasher.ComputeHash(bytes)).Replace("-", string.Empty).ToLower();
            }
        }
    }
}
