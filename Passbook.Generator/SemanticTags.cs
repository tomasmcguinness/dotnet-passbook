using Passbook.Generator.Tags;
using System.Collections.Generic;
using System.Text.Json;

namespace Passbook.Generator;

public class SemanticTags : List<SemanticTag>
{
    public void Write(Utf8JsonWriter writer)
    {
        writer.WritePropertyName("semantics");
        writer.WriteStartObject();
        
        foreach (var tag in this)
        {
            tag.Write(writer);
        }

        writer.WriteEndObject();
    }
}
