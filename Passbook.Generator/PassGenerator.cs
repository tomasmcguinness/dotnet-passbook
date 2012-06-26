using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Passbook.Generator
{
  public class PassbookGenerator
  {
    public Pass Generate(PassGeneratorRequest request)
    {
      if (request == null)
      {
        throw new ArgumentNullException("request");
      }

      CreatePackage(request);

      return new Pass();
    }

    private void CreatePackage(PassGeneratorRequest request)
    {
      string tempPath = Path.GetTempPath();
      string passFile = Path.Combine(tempPath, "pass.json");
      using (StreamWriter sr = File.CreateText(passFile))
      {
        JsonSerializer serializer = new JsonSerializer();
        serializer.Serialize(sr, request);
      }
    }
  }
}
