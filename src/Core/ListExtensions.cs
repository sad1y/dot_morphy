using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Dawg
{
    public static class ListExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RemoveLast<T>(this List<T> list)
        {
            if (list.Count == 0)
                return;
            
            list.RemoveAt(list.Count - 1);
        }
    }
}