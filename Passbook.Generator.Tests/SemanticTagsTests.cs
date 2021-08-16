using Newtonsoft.Json;
using Passbook.Generator.Tags;
using System.IO;
using System.Text;
using Xunit;

namespace Passbook.Generator.Tests
{
    public class SemanticTagsTests
    {
        [Fact]
        public void EnsureSemanticFieldsIsGeneratedCorrectly()
        {
            PassGeneratorRequest request = new PassGeneratorRequest();
            request.SemanticTags.Add(new AirlineCode("EX"));
            request.SemanticTags.Add(new Balance("1000", "GBP"));

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

                    var semantics = json["semantics"];

                    var airlineCode = semantics.airlineCode;
                    Assert.Equal("EX", (string)airlineCode);

                    var balance = semantics.balance;
                    Assert.Equal("1000", (string)balance.amount);
                    Assert.Equal("GBP", (string)balance.currencyCode);
                }
            }
        }
    }
}
