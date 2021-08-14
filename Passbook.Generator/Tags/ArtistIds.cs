using Newtonsoft.Json;

namespace Passbook.Generator.Tags
{
    public class ArtistIds : SemanticTag
    {
        private readonly string[] _artistIds;

        public ArtistIds(params string[] artistIds)
        {
            _artistIds = artistIds;
        }

        public override void Write(JsonWriter writer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("airlineCode");
            writer.WriteValue(_airlineCode);

            writer.WriteEndObject();
        }
    }
}
