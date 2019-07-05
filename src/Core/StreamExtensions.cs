using System;
using System.Buffers;
using System.IO;
using System.Threading.Tasks;

namespace Dawg
{
    public static class StreamExtensions
    {
        public static async Task<T[]> ReadAsAsync<T>(this Stream stream,
            Action<byte[], int, Memory<T>> serializeCallback,
            int size = 1024 * 1024)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (serializeCallback == null) throw new ArgumentNullException(nameof(serializeCallback));
            if (size < 1024 * 1024) throw new ArgumentOutOfRangeException(nameof(size));

            var buffer = new byte[4096];
            var offset = 0;

            var mem = MemoryPool<T>.Shared.Rent(size);

            int count;
            while ((count = await stream.ReadAsync(buffer, offset, buffer.Length)) > 0)
            {
                // need to increase rented mem?
                if (mem.Memory.Length < offset + count)
                {
                    var newMem = MemoryPool<T>.Shared.Rent(mem.Memory.Length + size);
                    mem.Memory.TryCopyTo(newMem.Memory);
                    mem.Dispose();
                    mem = newMem;
                }

                serializeCallback(buffer, count, mem.Memory);

                offset += count;
            }

            var result = mem.Memory.ToArray();

            mem.Dispose();

            return result;
        }
    }
}