using System;
using System.Text.Json;

namespace Passbook.Generator.Tags;

public abstract class SemanticTagBaseValue : SemanticTag
{
    private readonly object _value;

    public SemanticTagBaseValue(string tag, string value) : base(tag)
    {
        _value = value;
    }

    public SemanticTagBaseValue(string tag, bool value) : base(tag)
    {
        _value = value;
    }

    public SemanticTagBaseValue(string tag, double value) : base(tag)
    {
        _value = value;
    }

    public override void WriteValue(Utf8JsonWriter writer)
    {
        switch (_value)
        {
            case string s:
                writer.WriteStringValue(s);
                break;
            case bool b:
                writer.WriteBooleanValue(b);
                break;
            case double d:
                writer.WriteNumberValue(d);
                break;
            case float f:
                writer.WriteNumberValue(f);
                break;
            case decimal m:
                writer.WriteNumberValue(m);
                break;
            case int i:
                writer.WriteNumberValue(i);
                break;
            case long l:
                writer.WriteNumberValue(l);
                break;
        }
    }
}