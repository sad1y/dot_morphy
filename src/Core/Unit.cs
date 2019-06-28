namespace Dawg
{
    public static class Unit
    {
        private const uint PrecisionMask = 0xFFFF_FFFF;
        private const uint IsLeafBit= 0x8000_0000;         // 1 << 31
        private const uint HasLeafBit = 0x0000_0100;       // 1 << 8
        private const uint ExtensionBit = 0x0000_0200;     // 1 << 9
        
        public static bool HasLeaf(this uint index) => (index & HasLeafBit) != 0;
        public static uint Value(this uint index) => index & (IsLeafBit ^ PrecisionMask);
        public static uint Label(this uint index) => index & (IsLeafBit | 0xFF);
        public static uint Offset(this uint index) => (index >> 10) << (int)((index & ExtensionBit) >> 6);
    }
}