using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Dawg
{
    public class Graph
    {
        private readonly Dictionary _dict;

        public static Graph FromStream(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            return new Graph(Dictionary.Create(stream));
        }

        public Graph(Dictionary dict)
        {
            _dict = dict ?? throw new ArgumentNullException(nameof(dict));
        }

        
        public IEnumerable<string> Prefixes(byte[] text)
        {
            var index = Dictionary.Root;

            for (var i = 0; i < text.Length; i++)
            {
                var next = _dict.FollowChar(text[i], index);
                
                if(!next.HasValue) 
                    yield break;

                index = next.Value;

                if (_dict.HasValue(index))
                    yield return Encoding.UTF8.GetString(text, 0, i);
            }
        }

        public IEnumerable<string> SortedPrefix(byte[] text) => Prefixes(text).OrderBy(f => f.Length);


        public uint? FollowBytes(char ch, uint index) => _dict.FollowBytes(ch, index);

    }
}