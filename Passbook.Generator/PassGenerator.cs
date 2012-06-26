using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
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
      string passFileAndPath = Path.Combine(tempPath, "pass.json");

      using (StreamWriter sr = File.CreateText(passFileAndPath))
      {
        JsonSerializer serializer = new JsonSerializer();
        serializer.Serialize(sr, request);
      }

      return tempPath;
    }
  }
}
