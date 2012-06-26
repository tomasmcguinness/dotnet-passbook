using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

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

      return new Pass();
    }

    private string ZipPackage(string pathToPackage)
    {
      ZipFile.CreateFromDirectory(pathToPackage, pathToPackage + "\\..\\pass.pkpass");
      return "pass.pkpass";
    }

    private string CreatePackage(PassGeneratorRequest request)
    {
      string tempPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName(), "contents");
      Directory.CreateDirectory(tempPath);

      CreatePassFile(request, tempPath);
      GenerateManifestFile(request, tempPath);

      return tempPath;
    }

    private void CreatePassFile(PassGeneratorRequest request, string tempPath)
    {
      string passFileAndPath = Path.Combine(tempPath, "pass.json");

      using (StreamWriter sr = File.CreateText(passFileAndPath))
      {
        JsonSerializer serializer = new JsonSerializer();
        serializer.Serialize(sr, request);
      }
    }

    private void GenerateManifestFile(PassGeneratorRequest request, string tempPath)
    {
      string manifestFileAndPath = Path.Combine(tempPath, "manifest.json");
      string[] filesToInclude = Directory.GetFiles(tempPath);

      using (StreamWriter sw = new StreamWriter(File.Open(manifestFileAndPath, FileMode.Create)))
      {
        using (JsonWriter jsonWriter = new JsonTextWriter(sw))
        {
          foreach (var fileNameWithPath in filesToInclude)
          {
            string fileName = Path.GetFileName(fileNameWithPath);
            string hash = GetHashForFile(fileName);

            jsonWriter.Formatting = Formatting.Indented;
            jsonWriter.WriteStartObject();

            jsonWriter.WritePropertyName(fileName);
            jsonWriter.WriteValue(hash);
          }
        }
      }

      SignManigestFile(request, manifestFileAndPath);
    }

    private void SignManigestFile(PassGeneratorRequest request, string manifestFileAndPath)
    {
      throw new NotImplementedException();
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
