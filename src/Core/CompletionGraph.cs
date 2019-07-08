using System;
using System.IO;
using System.Collections.Generic;

namespace Dawg
{
    public class CompletionGraph<TValue> where TValue : struct
    {
        private readonly Graph _graph;
        private readonly Guide _guide;

        public static CompletionGraph<TValue> FromStream(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            return new CompletionGraph<TValue>(
                Dictionary.Create(stream),
                Guide.Create(stream)
            );
        }

        private CompletionGraph(Graph graph, Guide guide)
        {
            _graph = graph ?? throw new ArgumentNullException(nameof(graph));
            _guide = guide ?? throw new ArgumentNullException(nameof(guide));
        }

        public void SimilarItems(string text, Dictionary<char, char> replaces)
        {
        }
        
        private IEnumerable<SimilarItem<TValue>> SimilarItems(string prefix, string key, uint index, Dictionary<char, char> replaces)
        {
            var start = prefix.Length;
            var subkey = prefix.AsSpan().Slice(start);

            var wordPostion = start;
            
            //         for b_step in subkey.split("").filter(|v| !v.is_empty()) {

            for (var i = 0; i < subkey.Length; i++)
            {
                if (subkey[i] == '') continue;


                if (replaces.TryGetValue(subkey[i], out var replace))
                {
                    
                    /* 
                    if let Some(next_index) = self.dawg.dict.follow_bytes(replace_char, index) {
                        log::trace!(r#" next_index: {}"#, next_index);
                        let prefix = format!(
                            "{}{}{}",
                            current_prefix,
                            &key[start_pos..word_pos],
                            replace_char
                        );
                        self.similar_items_(result, &prefix, key, next_index, replace_chars);
                    };
                }
*/
                    var nextIndex = _graph.FollowBytes(replace, index);
                    
                    
                }
            }
        }
    }

    public class SimilarItem<TValue>
    {
        public string Str;
        public List<TValue> Values;
    }
}