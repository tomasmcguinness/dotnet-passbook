using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Passbook.Generator.Fields;
using System;
using System.IO;
using System.Text;

namespace Passbook.Generator.Tests
{
    [TestClass]
    public class GeneratorTests
    {
        [TestMethod]
        public void EnsurePassIsGeneratedCorrectly()
        {
            PassGeneratorRequest request = new PassGeneratorRequest();
            request.ExpirationDate = new DateTime(2018, 1, 1, 0, 0, 0, DateTimeKind.Local);
            request.Nfc = new Nfc("My NFC Message", "SKLSJLKJ");

            DateTime offset = new DateTime(2018, 01, 05, 12, 00, 0);
            TimeZoneInfo zone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTimeOffset offsetConverted = new DateTimeOffset(offset, zone.GetUtcOffset(offset));

            request.RelevantDate = offsetConverted;

            request.AddAuxiliaryField(new StandardField()
            {
                Key = "aux-1",
                Value = "Test",
                Label = "Label",
                Row = 1
            });

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

                    var nfcPayload = (JToken)json["nfc"];
                    var nfcMessage = (string)nfcPayload["message"];
                    Assert.AreEqual("My NFC Message", nfcMessage);

                    var genericKeys = json["generic"];
                    Assert.AreEqual(1, genericKeys["auxiliaryFields"].Count);

                    var auxField = genericKeys["auxiliaryFields"][0];

                    Assert.AreEqual("aux-1", (string)auxField["key"]);
                    Assert.AreEqual("Test", (string)auxField["value"]);
                    Assert.AreEqual("Label", (string)auxField["label"]);
                    Assert.AreEqual(1, (int)auxField["row"]);

                }
            }
        }
    }
}
