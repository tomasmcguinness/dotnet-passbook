using Passbook.Generator.Exceptions;
using Passbook.Generator.Fields;
using System.Text;
using System.Text.Json;
using TimeZoneConverter;
using Xunit;

namespace Passbook.Generator.Tests;

public class GeneratorTests
{
    [Fact]
    public void EnsurePassIsGeneratedCorrectly()
    {
        var request = new PassGeneratorRequest();
        request.ExpirationDate = new DateTime(2018, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        request.Nfc = new Nfc("My NFC Message", "SKLSJLKJ");

        var offset = new DateTime(2018, 01, 05, 12, 00, 0);
        var zone = TZConvert.GetTimeZoneInfo("Eastern Standard Time");
        var offsetConverted = new DateTimeOffset(offset, zone.GetUtcOffset(offset));

        request.RelevantDate = offsetConverted;

        request.AddRelevantDate(offsetConverted);

        request.AddAuxiliaryField(new StandardField()
        {
            Key = "aux-1",
            Value = "Test",
            Label = "Label",
            Row = 1
        });

        request.AssociatedStoreIdentifiers.Add(long.MaxValue);

        using var ms = new MemoryStream();
        var opts = new JsonWriterOptions { Indented = true };
        using (var writer = new Utf8JsonWriter(ms, opts))
        
        request.Write(writer);

        string jsonString = Encoding.UTF8.GetString(ms.ToArray());

        dynamic json = JsonSerializer.Deserialize<dynamic>(jsonString)!;

        json.TryGetProperty("expirationDate", out JsonElement expirationDate);
        Assert.Equal("2018-01-01T00:00:00+00:00", expirationDate.GetString());

        json.TryGetProperty("relevantDate", out JsonElement relevantDate);
        Assert.Equal("2018-01-05T12:00:00-05:00", relevantDate.GetString());

        json.TryGetProperty("nfc", out JsonElement nfcPayload);
        nfcPayload.TryGetProperty("message", out JsonElement nfcMessage);
        Assert.Equal("My NFC Message", nfcMessage.GetString());

        json.TryGetProperty("generic", out JsonElement genericKeys);


        json.TryGetProperty("relevantDates", out JsonElement relevantDates);
        Assert.Equal(1, relevantDates.GetArrayLength());

        var relevantDateItem = relevantDates.EnumerateArray().ToArray()[0];
        relevantDateItem.TryGetProperty("date", out JsonElement date);
        Assert.Equal("2018-01-05T12:00:00-05:00", date.GetString());

        genericKeys.TryGetProperty("auxiliaryFields", out JsonElement auxFields);
        Assert.Equal(1, auxFields.GetArrayLength());

        var auxField = auxFields.EnumerateArray().ToArray()[0];

        auxField.TryGetProperty("key", out JsonElement key);
        Assert.Equal("aux-1", key.GetString());

        auxField.TryGetProperty("value", out JsonElement value);
        Assert.Equal("Test", value.GetString());

        auxField.TryGetProperty("label", out JsonElement label);
        Assert.Equal("Label", label.GetString());

        auxField.TryGetProperty("row", out JsonElement row);
        Assert.Equal(1, row.GetInt32());

        json.TryGetProperty("associatedStoreIdentifiers", out JsonElement associatedAppIdentifiersPayload);
        Assert.Equal(1, associatedAppIdentifiersPayload.GetArrayLength());

        var assocId = associatedAppIdentifiersPayload
            .EnumerateArray()
            .ToArray()[0];
        Assert.Equal(long.MaxValue, assocId.GetInt64());
    }

    [Fact]
    public void EnsureDuplicteKeysThrowAnException()
    {
        PassGeneratorRequest request = new PassGeneratorRequest();

        request.AddAuxiliaryField(new StandardField()
        {
            Key = "aux-1",
            Value = "Test",
            Label = "Label",
        });

        Assert.Throws<DuplicateFieldKeyException>(() => request.AddHeaderField(new StandardField()
        {
            Key = "aux-1",
            Value = "Test",
            Label = "Label",
        }));
    }

    [Fact]
    public void EnsureFieldHasLocalTime()
    {
        var request = new PassGeneratorRequest
        {
            ExpirationDate = new DateTime(2018, 1, 1, 0, 0, 0, DateTimeKind.Local),
            Nfc = new Nfc("My NFC Message", "SKLSJLKJ")
        };

        var offset = new DateTime(2018, 01, 05, 12, 00, 0);
        var zone = TZConvert.GetTimeZoneInfo("Eastern Standard Time");
        var offsetConverted = new DateTimeOffset(offset, zone.GetUtcOffset(offset));

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

        using var ms = new MemoryStream();
        var opts = new JsonWriterOptions { Indented = true };
        using (var writer = new Utf8JsonWriter(ms, opts))

        request.Write(writer);

        string jsonString = Encoding.UTF8.GetString(ms.ToArray());
        Console.WriteLine(jsonString);

        dynamic json = JsonSerializer.Deserialize<dynamic>(jsonString)!;

        json.TryGetProperty("expirationDate", out JsonElement expirationDate);
        Assert.Equal("2018-01-01T00:00:00+02:00", expirationDate.GetString());

        json.TryGetProperty("relevantDate", out JsonElement relevantDate);
        Assert.Equal("2018-01-05T12:00:00-05:00", relevantDate.GetString());

        json.TryGetProperty("nfc", out JsonElement nfcPayload);
        nfcPayload.TryGetProperty("message", out JsonElement nfcMessage);
        Assert.Equal("My NFC Message", nfcMessage.GetString());

        json.TryGetProperty("generic", out JsonElement genericKeys);

        genericKeys.TryGetProperty("auxiliaryFields", out JsonElement auxFields);
        Assert.Equal(3, auxFields.EnumerateArray().Count());

        var auxField = auxFields.EnumerateArray()
            .ToArray()[0];

        auxField.TryGetProperty("key", out JsonElement key);
        Assert.Equal("aux-1", key.GetString());
        auxField.TryGetProperty("value", out JsonElement value);
        Assert.Equal("Test", value.GetString());
        auxField.TryGetProperty("label", out JsonElement label);
        Assert.Equal("Label", label.GetString());
        auxField.TryGetProperty("row", out JsonElement row);
        Assert.Equal(1, row.GetInt32());

        var dateTimeField = auxFields.EnumerateArray()
            .ToArray()[1];

        dateTimeField.TryGetProperty("key", out JsonElement datetimeKey);
        Assert.Equal("datetime-1", datetimeKey.GetString());

        dateTimeField.TryGetProperty("value", out JsonElement dateTimeValue);
        string expected1start = string.Format("{0:yyyy-MM-ddTHH:mm}", local);

        Assert.StartsWith(expected1start, dateTimeValue.GetString());
        Assert.DoesNotContain("Z", dateTimeValue.GetString());

        dateTimeField.TryGetProperty("label", out JsonElement dateTimeLabel);
        Assert.Equal("Label", dateTimeLabel.GetString());

        var utcDateTimeField = auxFields.EnumerateArray().ToArray()[2];

        utcDateTimeField.TryGetProperty("key", out JsonElement utcDateTimeFieldKey);
        Assert.Equal("datetime-2", utcDateTimeFieldKey.GetString());

        utcDateTimeField.TryGetProperty("value", out JsonElement utcDateTimeFieldValue);

        string expected2 = string.Format("{0:yyyy-MM-ddTHH:mm:ss}Z", utc);
        Assert.Equal(expected2, utcDateTimeFieldValue.GetString());

        utcDateTimeField.TryGetProperty("label", out JsonElement utcDateTimeFieldLabel);
        Assert.Equal("Label", utcDateTimeFieldLabel.GetString());
    }
}