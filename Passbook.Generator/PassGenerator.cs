using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Org.BouncyCastle.Cms;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509.Store;

namespace Passbook.Generator
{
    public class PassGenerator
    {
        public Pass Generate(PassGeneratorRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            string pathToPackage = CreatePackage(request);

            string pathToZip = ZipPackage(pathToPackage);

            return new Pass(pathToZip);
        }

        private string ZipPackage(string pathToPackage)
        {
            string output = pathToPackage + "\\..\\pass.pkpass";
            ZipFile.CreateFromDirectory(pathToPackage, output);
            return output;
        }

        private string CreatePackage(PassGeneratorRequest request)
        {
            string tempPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName(), "contents");
            Directory.CreateDirectory(tempPath);

            CopyImageFiles(request, tempPath);
            CreatePassFile(request, tempPath);
            GenerateManifestFile(request, tempPath);

            return tempPath;
        }

        private void CopyImageFiles(PassGeneratorRequest request, string tempPath)
        {
            string targetIconFileAndPath = Path.Combine(tempPath, Path.GetFileName(request.IconFile));
            string targetIconRetinaFileAndPath = Path.Combine(tempPath, Path.GetFileName(request.IconRetinaFile));

            File.Copy(request.IconFile, targetIconFileAndPath);
            File.Copy(request.IconRetinaFile, targetIconRetinaFileAndPath);

            string targetLogoFileAndPath = Path.Combine(tempPath, Path.GetFileName(request.LogoFile));
            string targetLogoRetinaFileAndPath = Path.Combine(tempPath, Path.GetFileName(request.LogoRetinaFile));

            File.Copy(request.LogoFile, targetLogoFileAndPath);
            File.Copy(request.LogoRetinaFile, targetLogoRetinaFileAndPath);

            string targetBackgroundFileAndPath = Path.Combine(tempPath, Path.GetFileName(request.BackgroundFile));
            string targetBackgroundRetinaFileAndPath = Path.Combine(tempPath, Path.GetFileName(request.BackgroundRetinaFile));

            File.Copy(request.BackgroundFile, targetBackgroundFileAndPath);
            File.Copy(request.BackgroundRetinaFile, targetBackgroundRetinaFileAndPath);
        }

        private void CreatePassFile(PassGeneratorRequest request, string tempPath)
        {
            string passFileAndPath = Path.Combine(tempPath, "pass.json");

            using (StreamWriter sr = File.CreateText(passFileAndPath))
            {
                using (JsonWriter writer = new JsonTextWriter(sr))
                {
                    writer.WriteStartObject();

                    WriteStandardKeys(writer, request);
                    WriteAppearanceKeys(writer, request);

                    WriteStyleSpecificKey(writer, request);

                    WritePrimaryFields(writer, request);
                    WriteSecondaryFields(writer, request);
                    WriteBackFields(writer, request);

                    WriteBarcode(writer, request);
                    WriteUrls(writer, request);

                    CloseStyleSpecificKey(writer);

                    writer.WriteEndObject();
                }
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

        private void WriteStyleSpecificKey(JsonWriter writer, PassGeneratorRequest request)
        {
            switch (request.Style)
            {
                case PassStyle.EventTicket:
                    writer.WritePropertyName("storeCard");
                    writer.WriteStartObject();
                    break;
                case PassStyle.StoreCard:
                    writer.WritePropertyName("eventTicket");
                    writer.WriteStartObject();
                    break;
            }
        }

        private void CloseStyleSpecificKey(JsonWriter writer)
        {
            writer.WriteEndObject();
        }

        private void WriteHeaderFields(JsonWriter writer, PassGeneratorRequest request)
        {
            writer.WritePropertyName("primaryFields");
            writer.WriteStartArray();

            foreach (var headerField in request.HeaderFields)
            {
                WriteField(writer, headerField);
            }

            writer.WriteEndArray();
            writer.WriteEndObject();
        }

        private void WritePrimaryFields(JsonWriter writer, PassGeneratorRequest request)
        {
        }

        private void WriteSecondaryFields(JsonWriter writer, PassGeneratorRequest request)
        {
        }

        private void WriteAuxiliaryFields(JsonWriter writer, PassGeneratorRequest request)
        {
        }

        private void WriteBackFields(JsonWriter writer, PassGeneratorRequest request)
        {
        }

        private void WriteField(JsonWriter writer, Field field)
        {
            writer.WriteStartObject();
            
            writer.WritePropertyName("key");
            writer.WriteValue(field.Key);

            writer.WritePropertyName("changeMessage");
            writer.WriteValue(field.ChangeMessage);

            writer.WritePropertyName("label");
            writer.WriteValue(field.Label);

            writer.WritePropertyName("textAlignment");
            writer.WriteValue(field.TextAlignment);

            //writer.WritePropertyName("value");
            //writer.write.WriteValue(field.);
            
            writer.WriteEndObject();
        }

        private void GenerateManifestFile(PassGeneratorRequest request, string tempPath)
        {
            string manifestFileAndPath = Path.Combine(tempPath, "manifest.json");
            string[] filesToInclude = Directory.GetFiles(tempPath);

            using (StreamWriter sw = new StreamWriter(File.Open(manifestFileAndPath, FileMode.Create)))
            {
                using (JsonWriter jsonWriter = new JsonTextWriter(sw))
                {
                    jsonWriter.Formatting = Formatting.Indented;
                    jsonWriter.WriteStartObject();

                    foreach (var fileNameWithPath in filesToInclude)
                    {
                        string fileName = Path.GetFileName(fileNameWithPath);
                        string hash = GetHashForFile(fileNameWithPath);

                        jsonWriter.WritePropertyName(fileName);
                        jsonWriter.WriteValue(hash.ToLower());
                    }
                }
            }

            SignManigestFile(request, manifestFileAndPath);
        }

        private void SignManigestFile(PassGeneratorRequest request, string manifestFileAndPath)
        {
            byte[] dataToSign = File.ReadAllBytes(manifestFileAndPath);

            X509Certificate2 card = GetCertificate(request);
            Org.BouncyCastle.X509.X509Certificate cert = DotNetUtilities.FromX509Certificate(card);
            Org.BouncyCastle.Crypto.AsymmetricKeyParameter privateKey = DotNetUtilities.GetKeyPair(card.PrivateKey).Private;

            X509Certificate2 appleCA = GetAppleCertificate();
            Org.BouncyCastle.X509.X509Certificate appleCert = DotNetUtilities.FromX509Certificate(appleCA);

            ArrayList intermediateCerts = new ArrayList();

            intermediateCerts.Add(appleCert);
            intermediateCerts.Add(cert);

            Org.BouncyCastle.X509.Store.X509CollectionStoreParameters PP = new Org.BouncyCastle.X509.Store.X509CollectionStoreParameters(intermediateCerts);
            Org.BouncyCastle.X509.Store.IX509Store st1 = Org.BouncyCastle.X509.Store.X509StoreFactory.Create("CERTIFICATE/COLLECTION", PP);

            CmsSignedDataGenerator generator = new CmsSignedDataGenerator();

            generator.AddSigner(privateKey, cert, CmsSignedDataGenerator.DigestSha1);
            generator.AddCertificates(st1);

            CmsProcessable content = new CmsProcessableByteArray(dataToSign);
            CmsSignedData signedData = generator.Generate(content, false);

            string outputDirectory = Path.GetDirectoryName(manifestFileAndPath);
            string signatureFileAndPath = Path.Combine(outputDirectory, "signature");

            File.WriteAllBytes(signatureFileAndPath, signedData.GetEncoded());
        }

        private X509Certificate2 GetAppleCertificate()
        {
            return GetSpecifiedCertificate("‎0950b6cd3d2f37ea246a1aaa20dfaadbd6fe1f75", StoreName.CertificateAuthority, StoreLocation.LocalMachine);
        }

        public static X509Certificate2 GetCertificate(PassGeneratorRequest request)
        {
            return GetSpecifiedCertificate(request.CertThumbprint, StoreName.My, request.CertLocation);
        }

        private static X509Certificate2 GetSpecifiedCertificate(string thumbPrint, StoreName storeName, StoreLocation storeLocation)
        {
            X509Store store = new X509Store(storeName, storeLocation);
            store.Open(OpenFlags.ReadOnly);

            X509Certificate2Collection certs = store.Certificates;

            if (certs.Count > 0)
            {
                for (int i = 0; i < certs.Count; i++)
                {
                    X509Certificate2 cert = certs[i];

                    Debug.WriteLine(cert.Thumbprint);

                    if (string.Compare(cert.Thumbprint, thumbPrint, true) == 0)
                    {
                        return certs[i];
                    }
                }
            }

            return null;
        }

        private string GetHashForFile(string fileAndPath)
        {
            SHA1CryptoServiceProvider oSHA1Hasher = new SHA1CryptoServiceProvider();
            byte[] hashBytes;
            using (FileStream fs = File.Open(fileAndPath, FileMode.Open))
            {
                hashBytes = oSHA1Hasher.ComputeHash(fs);
            }

            string hash = System.BitConverter.ToString(hashBytes);
            hash = hash.Replace("-", "");
            return hash;
        }
    }
}
