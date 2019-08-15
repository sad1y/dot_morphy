using System.Runtime.InteropServices;

namespace Dawg
{
    [StructLayout(LayoutKind.Sequential)]
    public struct U16_2
    {
        public ushort N1;
        public ushort N2;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct U16_3
    {
        public ushort N1;
        public ushort N2;
        public ushort N3;
    }
}