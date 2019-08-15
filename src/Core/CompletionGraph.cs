using System;
using System.Buffers;
using System.Buffers.Text;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Dawg
{
    public class CompletionGraphU16_2 : CompletionGraphBase<U16_2>
    {
        private CompletionGraphU16_2(Graph graph, Guide guide) : base(graph, guide)
        {
        }

        public static CompletionGraphU16_2 FromStream(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            return new CompletionGraphU16_2(
                Graph.FromStream(stream),
                Guide.Create(stream)
            );
        }

        protected override unsafe U16_2 FromBytes(byte[] key)
        {
            var buff = stackalloc byte[4];
            var buffSpan = new Span<byte>(buff, 4);

            if (Base64.DecodeFromUtf8(key, buffSpan, out _, out var written) !=
                OperationStatus.Done)
                throw new EncoderFallbackException("cannot parse struct from bytes");

            Debug.Assert(written == 4);

            var result = new U16_2
            {
                N1 = BitConverter.ToUInt16(buffSpan), 
                N2 = BitConverter.ToUInt16(buffSpan)
            };

            return result;
        }
    }
    
    public class CompletionGraphU16_3 : CompletionGraphBase<U16_3>
    {
        private CompletionGraphU16_3(Graph graph, Guide guide) : base(graph, guide)
        {
        }

        public static CompletionGraphU16_3 FromStream(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            return new CompletionGraphU16_3(
                Graph.FromStream(stream),
                Guide.Create(stream)
            );
        }

        protected override unsafe U16_3 FromBytes(byte[] key)
        {
            var buff = stackalloc byte[6];
            var buffSpan = new Span<byte>(buff, 6);

            if (Base64.DecodeFromUtf8(key, buffSpan, out _, out var written) !=
                OperationStatus.Done)
                throw new EncoderFallbackException("cannot parse struct from bytes");

            Debug.Assert(written == 6);

            var result = new U16_3
            {
                N1 = BitConverter.ToUInt16(buffSpan), 
                N2 = BitConverter.ToUInt16(buffSpan),
                N3 = BitConverter.ToUInt16(buffSpan)
            };

            return result;
        }
    }

    public abstract class CompletionGraphBase<TValue> where TValue : struct
    {
        private readonly Graph _graph;
        private readonly Guide _guide;


        protected CompletionGraphBase(Graph graph, Guide guide)
        {
            _graph = graph ?? throw new ArgumentNullException(nameof(graph));
            _guide = guide ?? throw new ArgumentNullException(nameof(guide));
        }

        public List<SimilarItem<TValue>> SimilarItems(string text, Dictionary<char, char> replaces = null)
        {
            var result = new List<SimilarItem<TValue>>();
            SimilarItems(result, "", text, Dictionary.Root, replaces);
            return result;
        }

        private unsafe void SimilarItems(
            List<SimilarItem<TValue>> result,
            ReadOnlySpan<char> prefix,
            ReadOnlySpan<char> key,
            uint index,
            Dictionary<char, char> replaces)
        {
            var start = prefix.Length;
            var subkey = key.Slice(start);

            var wordPosition = start;

            for (var i = 0; i < subkey.Length; i++)
            {
                if (subkey[i] == '\x0') continue; // TODO: do we need it?

                if (replaces.TryGetValue(subkey[i], out var replace))
                {
                    // perf: inline
                    var next = _graph.FollowBytes(replace, index);

                    if (next.HasValue)
                    {
                        // perf: ValueStringBuilder
                        var newPrefix = prefix.Concat(
                            key.Slice(start, wordPosition),
                            new ReadOnlySpan<char>(&replace, 1));

                        SimilarItems(result, newPrefix, key, index, replaces);
                    }
                }

                var nextIndex = _graph.FollowBytes(subkey[i], index);

                if (!nextIndex.HasValue)
                    return;

                index = nextIndex.Value;

                wordPosition++;
            }

            var prefixPosition = _graph.FollowBytes('\x01', index);

            if (!prefixPosition.HasValue) return;

            var foundedKey = prefix.Concat(subkey);
            var val = ValueForIndex(prefixPosition.Value);
            result.Add(new SimilarItem<TValue> {Key = foundedKey, Values = val});
        }

        private List<TValue> ValueForIndex(uint index)
        {
            var result = new List<TValue>();

            var completer = new Completer(_graph.Dictionary, _guide, index, Array.Empty<byte>());
            for (var key = completer.NextKey();
                key != null;
                key = completer.NextKey())
            {
                var value = FromBytes(key);
                result.Add(value);
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected abstract TValue FromBytes(byte[] key);
    }

    public class SimilarItem<TValue>
    {
        public string Key;
        public List<TValue> Values;
    }
}