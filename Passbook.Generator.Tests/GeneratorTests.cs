using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Passbook.Generator.Fields;
using System;
using System.IO;
using System.Text;
using Xunit;
using Microsoft.CSharp;
using TimeZoneConverter;

namespace Passbook.Generator.Tests
{
    public class GeneratorTests
    {
        [Fact]
        public void EnsurePassIsGeneratedCorrectly()
        {
            PassGeneratorRequest request = new PassGeneratorRequest();
            request.ExpirationDate = new DateTime(2018, 1, 1, 0, 0, 0, DateTimeKind.Local);
            request.Nfc = new Nfc("My NFC Message", "SKLSJLKJ");

            DateTime offset = new DateTime(2018, 01, 05, 12, 00, 0);
            TimeZoneInfo zone = TZConvert.GetTimeZoneInfo("Eastern Standard Time");
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

                    Assert.Equal("2018-01-01T00:00:00+00:00", (string)json["expirationDate"]);
                    Assert.Equal("2018-01-05T12:00:00-05:00", (string)json["relevantDate"]);

                    var nfcPayload = (JToken)json["nfc"];
                    var nfcMessage = (string)nfcPayload["message"];
                    Assert.Equal("My NFC Message", nfcMessage);

                    var genericKeys = json["generic"];
                    Assert.Equal(1, genericKeys["auxiliaryFields"].Count);

                    var auxField = genericKeys["auxiliaryFields"][0];

                    Assert.Equal("aux-1", (string)auxField["key"]);
                    Assert.Equal("Test", (string)auxField["value"]);
                    Assert.Equal("Label", (string)auxField["label"]);
                    Assert.Equal(1, (int)auxField["row"]);

                }
            }
        }

        [Fact]
        public void EnsureFieldHasLocalTime()
        {
            PassGeneratorRequest request = new PassGeneratorRequest();
            request.ExpirationDate = new DateTime(2018, 1, 1, 0, 0, 0, DateTimeKind.Local);
            request.Nfc = new Nfc("My NFC Message", "SKLSJLKJ");

            DateTime offset = new DateTime(2018, 01, 05, 12, 00, 0);
            TimeZoneInfo zone = TZConvert.GetTimeZoneInfo("Eastern Standard Time");
            DateTimeOffset offsetConverted = new DateTimeOffset(offset, zone.GetUtcOffset(offset));

            request.RelevantDate = offsetConverted;

            request.AddAuxiliaryField(new StandardField()
            {
                Key = "aux-1",
                Value = "Test",
                Label = "Label",
                Row = 1
            });

            var local = DateTime.Now;
            local = new DateTime(local.Year, local.Month, local.Day, local.Hour, local.Minute, local.Second, local.Kind);
            request.AddAuxiliaryField(new DateField()
            {
                Key = "datetime-1",
                Value = local,
                Label = "Label",
            });

            var utc = DateTime.UtcNow;
            utc = new DateTime(utc.Year, utc.Month, utc.Day, utc.Hour, utc.Minute, utc.Second, utc.Kind);
            request.AddAuxiliaryField(new DateField()
            {
                Key = "datetime-2",
                Value = utc,
                Label = "Label",
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
                    Console.WriteLine(jsonString);
                    var settings = new JsonSerializerSettings { DateParseHandling = DateParseHandling.None };

                    dynamic json = JsonConvert.DeserializeObject(jsonString, settings);

                    Assert.Equal("2018-01-01T00:00:00+00:00", (string)json["expirationDate"]);
                    Assert.Equal("2018-01-05T12:00:00-05:00", (string)json["relevantDate"]);

                    var nfcPayload = (JToken)json["nfc"];
                    var nfcMessage = (string)nfcPayload["message"];
                    Assert.Equal("My NFC Message", nfcMessage);

                    var genericKeys = json["generic"];
                    Assert.Equal(3, genericKeys["auxiliaryFields"].Count);

                    var auxField = genericKeys["auxiliaryFields"][0];

                    Assert.Equal("aux-1", (string)auxField["key"]);
                    Assert.Equal("Test", (string)auxField["value"]);
                    Assert.Equal("Label", (string)auxField["label"]);
                    Assert.Equal(1, (int)auxField["row"]);

                    var datetimeField = genericKeys["auxiliaryFields"][1];
                    Assert.Equal("datetime-1", (string)datetimeField["key"]);
                    string datetime1 = (string)datetimeField["value"];
                    string expected1start = string.Format("{0:yyyy-MM-ddTHH:mm}", local);

                    Assert.StartsWith(expected1start, datetime1);
                    Assert.DoesNotContain("Z", datetime1);
                    Assert.Equal("Label", (string)datetimeField["label"]);


                    var utcdatetimeField = genericKeys["auxiliaryFields"][2];
                    Assert.Equal("datetime-2", (string)utcdatetimeField["key"]);
                    string datetime2 = (string)utcdatetimeField["value"];
                    string expected2 = string.Format("{0:yyyy-MM-ddTHH:mm:ss}Z", utc);

                    Assert.Equal(expected2, datetime2);
                    Assert.Equal("Label", (string)utcdatetimeField["label"]);

                }
            }
        }
    }
}
