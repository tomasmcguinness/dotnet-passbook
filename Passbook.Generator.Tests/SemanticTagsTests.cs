using Passbook.Generator.Tags;
using System.Text;
using System.Text.Json;
using Xunit;

namespace Passbook.Generator.Tests;

public class SemanticTagsTests
{
    [Fact]
    public void EnsureSemanticFieldsIsGeneratedCorrectly()
    {
        PassGeneratorRequest request = new PassGeneratorRequest();
        request.SemanticTags.Add(new AirlineCode("EX"));
        request.SemanticTags.Add(new Balance("1000", "GBP"));

        using var ms = new MemoryStream();
        var options = new JsonWriterOptions { Indented = true };
        using (var writer = new Utf8JsonWriter(ms, options))
        
        request.Write(writer);

        string jsonString = Encoding.UTF8.GetString(ms.ToArray());

        using var doc = JsonDocument.Parse(jsonString);
        var root = doc.RootElement;

        if (!root.TryGetProperty("semantics", out JsonElement semantics))
        {
            Assert.Fail("semantics not found");
        }

        if (!semantics.TryGetProperty("airlineCode", out JsonElement airlineCode))
        {
            Assert.Fail("airlineCode not found");
        }
        Assert.Equal("EX", airlineCode.GetString());

        if (!semantics.TryGetProperty("balance", out JsonElement balance))
        {
            Assert.Fail("balance not found");
        }

        if (!balance.TryGetProperty("amount", out JsonElement amount))
        {
            Assert.Fail("amount not found");
        }
        Assert.Equal("1000", amount.GetString());
    }
}
