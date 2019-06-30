using System.IO;
using System.Threading.Tasks;

namespace Dawg
{
    public struct GuideEntry
    {
        public byte Root;
        public byte Sibling;
    }
    
    public class Guide
    {
        public uint Root;
        public GuideEntry[] Units;

        public Guide(GuideEntry[] entries)
        {
            Root = 0;
            Units = entries;
        }

        public static async Task<Guide> Create(Stream stream)
        {
            var memOffset = 0;
            var units = await stream.ReadAsAsync<GuideEntry>((buffer, count, mem) =>
            {
                for (var i = 0; i < count; i += 2)
                {
                    mem.Span[memOffset++] = new GuideEntry { Root = buffer[i], Sibling = buffer[i+1]};
                }
            });
            
            return new Guide(units);
        }
    }
}