using Newtonsoft.Json;
using Passbook.Generator.Tags;
using System.Collections.Generic;
using System.Text;

namespace Passbook.Generator
{
    public class SemanticTags : List<SemanticTag>
    {
        public void Write(JsonWriter writer)
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
}
