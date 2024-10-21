using System.Text.Json;
using Passbook.Generator.Exceptions;

namespace Passbook.Generator.Fields;

public class StandardField : Field
{
    public StandardField()
        : base()
    { }

    public StandardField(string key, string label, string value)
        : base(key, label)
    {
        Value = value;
    }

    public StandardField(string key, string label, string value, string attributedValue, DataDetectorTypes dataDetectorTypes)
        : this(key, label, value)
    {
        AttributedValue = attributedValue;
        DataDetectorTypes = dataDetectorTypes;
    }

    public string Value { get; set; }

    protected override void WriteValue(Utf8JsonWriter writer)
    {
        if (Value == null)
        {
            throw new RequiredFieldValueMissingException(Key);
        }

        writer.WriteStringValue(Value);
    }

    public override void SetValue(object value)
    {
        Value = value as string;
    }

    public override bool HasValue
    {
        get { return !string.IsNullOrEmpty(Value); }
    }
}
