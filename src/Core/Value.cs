using System.Runtime.InteropServices;

namespace Dawg
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Bits4
    {
        public ushort N1;
        public ushort N2;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct Bits6
    {
        public ushort N1;
        public ushort N2;
        public ushort N3;
    }
}