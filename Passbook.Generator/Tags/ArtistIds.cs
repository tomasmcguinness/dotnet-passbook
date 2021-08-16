using Newtonsoft.Json;

namespace Passbook.Generator.Tags
{
    public class ArtistIds : SemanticTag
    {
        private readonly string[] _artistIds;

        public ArtistIds(params string[] artistIds) : base("artistIds")
        {
            _artistIds = artistIds;
        }

        public override void WriteValue(JsonWriter writer)
        {
            writer.WriteValue(_artistIds);
        }
    }
}
