using System.IO;

namespace Dawg
{
    public struct GuideEntry
    {
        public byte Child;
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

        public static Guide Create(Stream stream)
        {
            var memOffset = 0;
            var units = stream.ReadAs<GuideEntry>((buffer, count, mem) =>
            {
                for (var i = 0; i < count; i += 2)
                {
                    mem.Span[memOffset++] = new GuideEntry { Child = buffer[i], Sibling = buffer[i+1]};
                }
            });
            
            return new Guide(units);
        }
    }
}