using Passbook.Generator.Fields;
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

    [Fact]
    public void EnsurePosterEventTicketFieldsAreGeneratedCorrectly()
    {
        PassGeneratorRequest request = new PassGeneratorRequest();
        request.PreferredStyleSchemes.Add(PreferredStyleScheme.PosterEventTicket);
        request.PreferredStyleSchemes.Add(PreferredStyleScheme.EventTicket);

        request.SemanticTags.Add(new EventName("General event name"));
        request.SemanticTags.Add(new VenueName("Venue name"));
        request.SemanticTags.Add(new VenueRegionName("City or hosting name"));
        request.SemanticTags.Add(new VenueRoom("Venue room"));

        using var ms = new MemoryStream();
        var options = new JsonWriterOptions { Indented = true };
        using (var writer = new Utf8JsonWriter(ms, options))

        request.Write(writer);

        string jsonString = Encoding.UTF8.GetString(ms.ToArray());

        using var doc = JsonDocument.Parse(jsonString);
        var root = doc.RootElement;

        if (!root.TryGetProperty("preferredStyleSchemes", out JsonElement preferredStyleSchemes))
        {
            Assert.Fail("preferredStyleSchemes not found");
        }
        Assert.Equal(JsonValueKind.Array, preferredStyleSchemes.ValueKind);
        Assert.Equal("posterEventTicket", preferredStyleSchemes[0].GetString());
        Assert.Equal("eventTicket", preferredStyleSchemes[1].GetString());

        if (!root.TryGetProperty("semantics", out JsonElement semantics))
        {
            Assert.Fail("semantics not found");
        }

        Assert.Equal("General event name", semantics.GetProperty("eventName").GetString());
        Assert.Equal("Venue name", semantics.GetProperty("venueName").GetString());
        Assert.Equal("City or hosting name", semantics.GetProperty("venueRegionName").GetString());
        Assert.Equal("Venue room", semantics.GetProperty("venueRoom").GetString());
    }

    [Fact]
    public void EnsurePosterGenericFallsBackToGenericWithSameFields()
    {
        PassGeneratorRequest request = new PassGeneratorRequest();
        request.Style = PassStyle.PosterGeneric;
        request.FallbackStyle = PassStyle.Generic;
        request.PreferredStyleSchemes.Add(PreferredStyleScheme.PosterGeneric);
        request.PreferredStyleSchemes.Add(PreferredStyleScheme.Generic);

        request.AddPrimaryField(new StandardField("balanceField", "Gift Card", "$100"));
        request.AddSecondaryField(new StandardField("cardholderName", "Cardholder", "CardHolder Name"));

        using var ms = new MemoryStream();
        var options = new JsonWriterOptions { Indented = true };
        using (var writer = new Utf8JsonWriter(ms, options))

        request.Write(writer);

        string jsonString = Encoding.UTF8.GetString(ms.ToArray());

        using var doc = JsonDocument.Parse(jsonString);
        var root = doc.RootElement;

        if (!root.TryGetProperty("preferredStyleSchemes", out JsonElement preferredStyleSchemes))
        {
            Assert.Fail("preferredStyleSchemes not found");
        }
        Assert.Equal("posterGeneric", preferredStyleSchemes[0].GetString());
        Assert.Equal("generic", preferredStyleSchemes[1].GetString());

        if (!root.TryGetProperty("posterGeneric", out JsonElement posterGeneric))
        {
            Assert.Fail("posterGeneric not found");
        }
        Assert.Equal("$100", posterGeneric.GetProperty("primaryFields")[0].GetProperty("value").GetString());
        Assert.Equal("CardHolder Name", posterGeneric.GetProperty("secondaryFields")[0].GetProperty("value").GetString());

        if (!root.TryGetProperty("generic", out JsonElement generic))
        {
            Assert.Fail("generic fallback dictionary not found");
        }
        Assert.Equal("$100", generic.GetProperty("primaryFields")[0].GetProperty("value").GetString());
        Assert.Equal("CardHolder Name", generic.GetProperty("secondaryFields")[0].GetProperty("value").GetString());
    }
}
