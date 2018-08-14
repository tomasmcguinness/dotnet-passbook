using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Passbook.Generator.Tests
{
    [TestClass]
    public class GeneratorTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            PassGeneratorRequest request = new PassGeneratorRequest();
            request.ExpirationDate = new DateTime(2018, 1, 1, 0, 0, 0, DateTimeKind.Local);

            DateTime offset = new DateTime(2018, 01, 05, 12, 00, 0);
            TimeZoneInfo zone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTimeOffset offsetConverted = new DateTimeOffset(offset, zone.GetUtcOffset(offset));

            request.RelevantDate = offsetConverted;

            using (MemoryStream ms = new MemoryStream())
            {
                using (StreamWriter sr = new StreamWriter(ms))
                {
                    using (JsonWriter writer = new JsonTextWriter(sr))
                    {
                        writer.Formatting = Formatting.Indented;
                        request.Write(writer);
                    }

                    string jsonString = Encoding.UTF8.GetString(ms.ToArray());

                    var settings = new JsonSerializerSettings { DateParseHandling = DateParseHandling.None };

                    dynamic json = JsonConvert.DeserializeObject(jsonString, settings);

                    Assert.AreEqual("2018-01-01T00:00:00+00:00", (string)json["expirationDate"]);
                    Assert.AreEqual("2018-01-05T12:00:00-05:00", (string)json["relevantDate"]);
                }
            }
        }
    }
}
