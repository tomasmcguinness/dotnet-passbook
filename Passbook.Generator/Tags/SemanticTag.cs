using Newtonsoft.Json;

namespace Passbook.Generator.Tags
{
    public abstract class SemanticTag
    {
        public abstract void Write(JsonWriter writer);
    }
}