using System.Text.Json;

namespace Passbook.Generator.Tags;

public abstract class SemanticTag(string tag)
{
    public string Tag { get; } = tag;

    public void Write(Utf8JsonWriter writer)
    {
        writer.WritePropertyName(Tag);
        WriteValue(writer);
    }

    public abstract void WriteValue(Utf8JsonWriter writer);
}