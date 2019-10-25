using System.Collections.Generic;

namespace OpenCorpora
{
    public class ParadigmEntry
    {
        public ushort PrefixId;
        public ushort TagId;
        public ushort SuffixId;

        public static IEnumerable<ParadigmEntry> Build(ushort[] paradigm)
        {
            if(paradigm == null || paradigm.Length == 0)
                yield break;
                
            var len = paradigm.Length / 3;

            for (var i = 0; i < paradigm.Length; i++)
            {
                yield return new ParadigmEntry
                {
                    SuffixId = paradigm[i],
                    TagId = paradigm[i + len],
                    PrefixId = paradigm[i + len * 2],
                };
            }
        } 
    }
}