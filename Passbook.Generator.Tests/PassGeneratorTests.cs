using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Passbook.Generator.Tests
{
  [TestClass]
  public class PassGeneratorTests
  {
    [TestMethod]
    public void RealTest()
    {
      PassGeneratorRequest request = new PassGeneratorRequest();
      request.Description = "My first pass";

      PassGenerator generator = new PassGenerator();
      generator.Generate(request);
    }
  }
}
