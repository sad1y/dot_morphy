using Utf8Json;

namespace OpenCorpora.Grammeme
{
    public class Reg
    {
        public string Name { get; set; }
        public string Parent { get; set; }
        public string Alias { get; set; }
        public string Description { get; set; }

        public static Reg FromJson(ref JsonReader reader) => JsonSerializer.Deserialize<Reg>(ref reader);
    }
}