using System;
using System.Buffers;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Dawg
{
    public class Dictionary
    {
        private readonly uint[] _units;
        private const uint Root = 0;

        // do not expose ctor because the only way for now is read a data from serialized stream
        private Dictionary(uint[] units) => _units = units;

        public bool HasValue(uint index) => _units[index].HasLeaf();
        
        public uint Value(uint index) {
            var offset = _units[index].Offset();
            var valueIndex = index ^ offset;
            return _units[valueIndex].Value();
        }

        public uint? TryValue(uint index) => HasValue(index) ? Value(index) : (uint?)null;
        
        /// Follows a transition
        public uint? FollowChar(byte label, uint index) {
                
            var unit = _units[index];
            var offset = unit.Offset();
            var nextIndex = index ^ offset ^ label;
            var leafLabel = _units[nextIndex].Label();

            return leafLabel == label ? nextIndex :  (uint?)null;
        }

        public uint? FollowBytes(string key, uint index)
        {
            uint? current = index;
            
            foreach (var b in Encoding.UTF8.GetBytes(key))
            {
                current = FollowChar(b, index);
            }

            return current;
        }

        public bool Contains(string key)
        {
            var index = FollowBytes(key, Root);
            return index.HasValue && HasValue(index.Value);
        }
        
        public uint? Find(string key)
        {
            var index = FollowBytes(key, Root);
            return index.HasValue ? TryValue(index.Value) : null;
        }

        public static async Task<Dictionary> Create(Stream stream, int size = 1024 * 1024)
        {
            if (size < 1024 * 1024) throw new ArgumentOutOfRangeException(nameof(size));
            
            var buffer = new byte[4096];
            var offset = 0;
            var memOffset = 0;

            var mem = MemoryPool<uint>.Shared.Rent(size); 
            {
                int count;
                while ((count = await stream.ReadAsync(buffer, offset, buffer.Length)) > 0)
                {
                    // need to increase rented mem?
                    if (mem.Memory.Length < offset + count)
                    {
                        var newMem = MemoryPool<uint>.Shared.Rent(mem.Memory.Length + size);
                        mem.Memory.TryCopyTo(newMem.Memory);
                        mem.Dispose();
                        mem = newMem;
                    }
                    
                    for (var i = 0; i < buffer.Length; i += 4)
                    {
                        var val = BitConverter.ToUInt32(buffer, i);
                        mem.Memory.Span[memOffset++] = val;
                    }

                    offset += count;
                }

                var units = mem.Memory.ToArray();
                
                return new Dictionary(units);
            }
        }
    }
}