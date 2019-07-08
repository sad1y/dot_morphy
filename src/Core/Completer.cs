using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dawg
{
    public class Completer
    {
        private readonly Dictionary _dict;
        private readonly Guide _guide;
        private readonly List<byte> _key;
        private readonly List<uint> _indexStack;
        private uint _lastIndex;

        public Completer(Dictionary dict, Guide guide, uint index, IEnumerable<byte> prefix)
        {
            if (!guide.Units.Any())
                throw new ArgumentException("guide entity is empty");

            _dict = dict;
            _guide = guide;

            _lastIndex = Dictionary.Root;
            _indexStack = new List<uint>(16) {index};
            _key = new List<byte>(prefix);
        }

        public uint Value() => _dict.Value(_lastIndex);

        public string NextKey()
        {
            var lastIndex = _indexStack[_indexStack.Count - 1];

            if (_lastIndex == Dictionary.Root)
                return FindTerminal(lastIndex);

            var entry = _guide.Units[lastIndex];
            if (entry.Child != 0)
            {
                var next = Follow(entry.Child, lastIndex);
                if (!next.HasValue) return null;
                lastIndex = next.Value;
            }
            else
            {
                var sibling = entry.Sibling;

                while (sibling == 0)
                {
                    sibling = _guide.Units[lastIndex].Sibling;
                    _key.RemoveLast();

                    if (_indexStack.Count == 0)
                        return null;
                    
                    _indexStack.RemoveLast();
                    lastIndex = _indexStack[_indexStack.Count - 1];
                }

                var next = Follow(sibling, lastIndex);
                if (!next.HasValue) return null;
                lastIndex = next.Value;
            }

            return FindTerminal(lastIndex);
        }

        private string FindTerminal(uint index)
        {
            while (!_dict.HasValue(index))
            {
                var childLabel = _guide.Units[index].Child;
                var next = Follow(childLabel, index);
                if (!next.HasValue)
                    return null;

                index = next.Value;
            }

            _lastIndex = index;
            return Encoding.UTF8.GetString(_key.ToArray());
        }

        private uint? Follow(byte label, uint index)
        {
            var nextIndex = _dict.FollowChar(label, index);
            if (!nextIndex.HasValue) return null;

            _key.Add(label);
            _indexStack.Add(nextIndex.Value);

            return nextIndex;
        }
    }
}