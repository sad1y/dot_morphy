using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Dawg
{
    internal abstract class CompletionGraph<TValue> where TValue : struct
    {
        private readonly Graph _graph;
        private readonly Guide _guide;

        public static CompletionGraph<TValue> FromStream(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            return new CompletionGraph<TValue>(
                Graph.FromStream(stream),
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
        
        private unsafe void SimilarItems(List<SimilarItem<TValue>> result, string prefix, string key, uint index, Dictionary<char, char> replaces)
        {
            var start = prefix.Length;
            var subkey = prefix.AsSpan().Slice(start);

            var wordPosition = start;
            
            for (var i = 0; i < subkey.Length; i++)
            {
                if (subkey[i] == '') continue;


                if (replaces.TryGetValue(subkey[i], out var replace))
                {
                    // perf: inline
                    var next = _graph.FollowBytes(replace, index);

                    if (next.HasValue)
                    {
                        // perf: ValueStringBuilder
                        var newPrefix = CreatePrefixWithReplace(replace);

                        SimilarItems(result, newPrefix, key, index, replaces);
                    }
                }

                var nextIndex = _graph.FollowBytes(subkey[i], index);
                
                if(!nextIndex.HasValue) 
                    return;

                index = nextIndex.Value;

                wordPosition ++;
            }

            var prefixPosition = _graph.FollowBytes('\x01', index);

            if (prefixPosition.HasValue)
            {
                var subkeyRef = subkey.GetPinnableReference();

                var foundedKey = string.Create(prefix.Length + subkey.Length, (prefix, subkeyRef, subkey.Length), (span, state) =>
                {
                    state.prefix.AsSpan().CopyTo(span);
                    var subkeySpan = MemoryMarshal.CreateSpan(ref state.subkeyRef, state.Length);
                    subkeySpan .CopyTo(span);
                });
                
                // foundedKey
                // var foundedKey = prefix + subkey;

                var val = ValueForIndex(prefixPosition.Value);
                result.Insert(0, new SimilarItem<TValue> { Str =  foundedKey, });
            }

            unsafe string CreatePrefixWithReplace(char replace)
            {
                var keySliceSize = wordPosition - start;
                var newPrefix = new string('\0', prefix.Length + keySliceSize + 1);
                            
                fixed (char* prefixPtr = prefix)
                fixed (char* newPrefixPtr = newPrefix)
                fixed (char* keyPtr = key)
                {
                    Unsafe.CopyBlock(newPrefixPtr, prefixPtr, (uint)prefix.Length * sizeof(char));
                    Unsafe.CopyBlock(newPrefixPtr + prefix.Length, keyPtr + start, (uint)keySliceSize);
                    newPrefixPtr[prefix.Length + keySliceSize + 1] = replace;
                }

                return newPrefix;
            } 
        }
        
        private List<TValue> ValueForIndex(uint index)
        {
            var result = new List<TValue>();
            var completer = new Completer(_graph.Dictionary, _guide, index, Array.Empty<byte>());

            var key = completer.NextKey();
            while (key != null)
            {
                key = completer.NextKey();
                var value = FromBytes(key);
                
                result.Add(value);
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected abstract TValue FromBytes(string key);
    }

    public class SimilarItem<TValue>
    {
        public string Str;
        public List<TValue> Values;
    }
}