using Newtonsoft.Json;

namespace Passbook.Generator.Tags
{
    /// <summary>
    /// An array of the Apple Music persistent ID for each artist performing at the event, in decreasing order of significance. 
    /// Use this key for any type of event ticket.
    /// </summary>
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
