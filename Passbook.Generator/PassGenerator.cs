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
using Passbook.Generator.Fields;

namespace Passbook.Generator
{
    public class PassGenerator
    {
        private byte[] passFile = null;
        private byte[] signatureFile = null;
        private byte[] manifestFile = null;
        private byte[] pkPassFile = null;

        private const string APPLE_CERTIFICATE_THUMBPRINT = "‎0950b6cd3d2f37ea246a1aaa20dfaadbd6fe1f75";

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
                    ZipArchiveEntry imageEntry = null;

                    if (request.Images.ContainsKey(PassbookImage.Icon))
                    {
                        imageEntry = archive.CreateEntry(@"icon.png");
                        using (BinaryWriter writer = new BinaryWriter(imageEntry.Open()))
                        {
                            writer.Write(request.Images[PassbookImage.Icon]);
                            writer.Flush();
                        }
                    }

                    if (request.Images.ContainsKey(PassbookImage.IconRetina))
                    {
                        imageEntry = archive.CreateEntry(@"icon@2x.png");
                        using (BinaryWriter writer = new BinaryWriter(imageEntry.Open()))
                        {
                            writer.Write(request.Images[PassbookImage.IconRetina]);
                            writer.Flush();
                        }
                    }

                    if (request.Images.ContainsKey(PassbookImage.Logo))
                    {
                        imageEntry = archive.CreateEntry(@"logo.png");
                        using (BinaryWriter writer = new BinaryWriter(imageEntry.Open()))
                        {
                            writer.Write(request.Images[PassbookImage.Logo]);
                            writer.Flush();
                        }

                        imageEntry = archive.CreateEntry(@"logo@2x.png");
                        using (BinaryWriter writer = new BinaryWriter(imageEntry.Open()))
                        {
                            writer.Write(request.Images[PassbookImage.LogoRetina]);
                            writer.Flush();
                        }
                    }

                    if (request.Images.ContainsKey(PassbookImage.Background))
                    {
                        imageEntry = archive.CreateEntry(@"background.png");
                        using (BinaryWriter writer = new BinaryWriter(imageEntry.Open()))
                        {
                            writer.Write(request.Images[PassbookImage.Background]);
                            writer.Flush();
                        }

                        imageEntry = archive.CreateEntry(@"background@2x.png");
                        using (BinaryWriter writer = new BinaryWriter(imageEntry.Open()))
                        {
                            writer.Write(request.Images[PassbookImage.BackgroundRetina]);
                            writer.Flush();
                        }
                    }

                    if (request.Images.ContainsKey(PassbookImage.Strip))
                    {
                        imageEntry = archive.CreateEntry(@"strip.png");
                        using (BinaryWriter writer = new BinaryWriter(imageEntry.Open()))
                        {
                            writer.Write(request.Images[PassbookImage.Strip]);
                            writer.Flush();
                        }

                        imageEntry = archive.CreateEntry(@"strip@2x.png");
                        using (BinaryWriter writer = new BinaryWriter(imageEntry.Open()))
                        {
                            writer.Write(request.Images[PassbookImage.StripRetina]);
                            writer.Flush();
                        }
                    }

                    if (request.Images.ContainsKey(PassbookImage.Thumbnail))
                    {
                        imageEntry = archive.CreateEntry(@"thumbnail.png");
                        using (BinaryWriter writer = new BinaryWriter(imageEntry.Open()))
                        {
                            writer.Write(request.Images[PassbookImage.Thumbnail]);
                            writer.Flush();
                        }

                        imageEntry = archive.CreateEntry(@"thumbnail@2x.png");
                        using (BinaryWriter writer = new BinaryWriter(imageEntry.Open()))
                        {
                            writer.Write(request.Images[PassbookImage.ThumbnailRetina]);
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
            CreatePassFile(request);
            GenerateManifestFile(request);
        }

        private void CreatePassFile(PassGeneratorRequest request)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (StreamWriter sr = new StreamWriter(ms))
                {
                    using (JsonWriter writer = new JsonTextWriter(sr))
                    {
                        request.Write(writer);
                    }

                    passFile = ms.ToArray();
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

                        if (request.Images.ContainsKey(PassbookImage.Icon))
                        {
                            hash = GetHashForBytes(request.Images[PassbookImage.Icon]);
                            jsonWriter.WritePropertyName(@"icon.png");
                            jsonWriter.WriteValue(hash.ToLower());
                        }

                        if (request.Images.ContainsKey(PassbookImage.IconRetina))
                        {
                            hash = GetHashForBytes(request.Images[PassbookImage.IconRetina]);
                            jsonWriter.WritePropertyName(@"icon@2x.png");
                            jsonWriter.WriteValue(hash.ToLower());
                        }

                        if (request.Images.ContainsKey(PassbookImage.Logo))
                        {
                            hash = GetHashForBytes(request.Images[PassbookImage.Logo]);
                            jsonWriter.WritePropertyName(@"logo.png");
                            jsonWriter.WriteValue(hash.ToLower());

                            hash = GetHashForBytes(request.Images[PassbookImage.LogoRetina]);
                            jsonWriter.WritePropertyName(@"logo@2x.png");
                            jsonWriter.WriteValue(hash.ToLower());
                        }

                        if (request.Images.ContainsKey(PassbookImage.Background))
                        {
                            hash = GetHashForBytes(request.Images[PassbookImage.Background]);
                            jsonWriter.WritePropertyName(@"background.png");
                            jsonWriter.WriteValue(hash.ToLower());

                            hash = GetHashForBytes(request.Images[PassbookImage.BackgroundRetina]);
                            jsonWriter.WritePropertyName(@"background@2x.png");
                            jsonWriter.WriteValue(hash.ToLower());
                        }

                        if (request.Images.ContainsKey(PassbookImage.Strip))
                        {
                            hash = GetHashForBytes(request.Images[PassbookImage.Strip]);
                            jsonWriter.WritePropertyName(@"strip.png");
                            jsonWriter.WriteValue(hash.ToLower());

                            hash = GetHashForBytes(request.Images[PassbookImage.StripRetina]);
                            jsonWriter.WritePropertyName(@"strip@2x.png");
                            jsonWriter.WriteValue(hash.ToLower());
                        }

                        if (request.Images.ContainsKey(PassbookImage.Thumbnail))
                        {
                            hash = GetHashForBytes(request.Images[PassbookImage.Thumbnail]);
                            jsonWriter.WritePropertyName(@"thumbnail.png");
                            jsonWriter.WriteValue(hash.ToLower());

                            hash = GetHashForBytes(request.Images[PassbookImage.ThumbnailRetina]);
                            jsonWriter.WritePropertyName(@"thumbnail@2x.png");
                            jsonWriter.WriteValue(hash.ToLower());
                        }

                        hash = GetHashForBytes(passFile);
                        jsonWriter.WritePropertyName(@"pass.json");
                        jsonWriter.WriteValue(hash.ToLower());
                    }

                    manifestFile = ms.ToArray();
                }

                SignManigestFile(request);
            }
        }

        private void SignManigestFile(PassGeneratorRequest request)
        {
            X509Certificate2 card = GetCertificate(request);

            if (card == null)
            {
                throw new FileNotFoundException("Certificate could not be found. Please ensure the thumbprint and cert location values are correct.");
            }

            Org.BouncyCastle.X509.X509Certificate cert = DotNetUtilities.FromX509Certificate(card);
            Org.BouncyCastle.Crypto.AsymmetricKeyParameter privateKey = DotNetUtilities.GetKeyPair(card.PrivateKey).Private;

            X509Certificate2 appleCA = GetAppleCertificate(request);
            Org.BouncyCastle.X509.X509Certificate appleCert = DotNetUtilities.FromX509Certificate(appleCA);

            ArrayList intermediateCerts = new ArrayList();

            intermediateCerts.Add(appleCert);
            intermediateCerts.Add(cert);

            Org.BouncyCastle.X509.Store.X509CollectionStoreParameters PP = new Org.BouncyCastle.X509.Store.X509CollectionStoreParameters(intermediateCerts);
            Org.BouncyCastle.X509.Store.IX509Store st1 = Org.BouncyCastle.X509.Store.X509StoreFactory.Create("CERTIFICATE/COLLECTION", PP);

            CmsSignedDataGenerator generator = new CmsSignedDataGenerator();

            generator.AddSigner(privateKey, cert, CmsSignedDataGenerator.DigestSha1);
            generator.AddCertificates(st1);

            CmsProcessable content = new CmsProcessableByteArray(manifestFile);
            CmsSignedData signedData = generator.Generate(content, false);

            signatureFile = signedData.GetEncoded();
        }

        private X509Certificate2 GetAppleCertificate(PassGeneratorRequest request)
        {
            if (request.AppleWWDRCACertificate == null)
            {
                return GetSpecifiedCertificateFromCertStore(APPLE_CERTIFICATE_THUMBPRINT, StoreName.CertificateAuthority, StoreLocation.LocalMachine);
            }
            else
            {
                return GetCertificateFromBytes(request.AppleWWDRCACertificate, null);
            }
        }

        public static X509Certificate2 GetCertificate(PassGeneratorRequest request)
        {
            if (request.Certificate == null)
            {
                return GetSpecifiedCertificateFromCertStore(request.CertThumbprint, StoreName.My, request.CertLocation);
            }
            else
            {
                return GetCertificateFromBytes(request.Certificate, request.CertificatePassword);
            }
        }

        private static X509Certificate2 GetSpecifiedCertificateFromCertStore(string thumbPrint, StoreName storeName, StoreLocation storeLocation)
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

        private static X509Certificate2 GetCertificateFromBytes(byte[] bytes, string password)
        {
            X509Certificate2 certificate = null;

            if (password == null)
            {
                certificate = new X509Certificate2(bytes);
            }
            else
            {
                certificate = new X509Certificate2(bytes, password, X509KeyStorageFlags.Exportable);
            }

            return certificate;
        }

        private string GetHashForBytes(byte[] bytes)
        {
            SHA1CryptoServiceProvider oSHA1Hasher = new SHA1CryptoServiceProvider();
            byte[] hashBytes;

            hashBytes = oSHA1Hasher.ComputeHash(bytes);

            string hash = System.BitConverter.ToString(hashBytes);
            hash = hash.Replace("-", "");
            return hash;
        }
    }
}
