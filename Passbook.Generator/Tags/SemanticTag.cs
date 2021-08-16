using Newtonsoft.Json;

namespace Passbook.Generator.Tags
{
    public abstract class SemanticTag
    {
        public SemanticTag(string tag)
        {
            Tag = tag;
        }

        public string Tag { get; }

        public void Write(JsonWriter writer)
        {
            writer.WritePropertyName(Tag);
            WriteValue(writer);
        }

        public abstract void WriteValue(JsonWriter writer);
    }
}