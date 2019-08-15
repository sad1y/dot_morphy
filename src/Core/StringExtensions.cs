using System;
using System.Buffers;

namespace Dawg
{
    public static class StringExtensions
    {
        public static unsafe string Concat(this ReadOnlySpan<char> span1, ReadOnlySpan<char> span2)
        {
            var tmp = stackalloc char[span1.Length + span2.Length];
            var span = new Span<char>(tmp, span1.Length + span2.Length);
            
            span1.CopyTo(span);
            span2.CopyTo(span);

            return span.ToString();
        }
        
        public static unsafe string Concat(this ReadOnlySpan<char> span1, ReadOnlySpan<char> span2, ReadOnlySpan<char> span3)
        {
            var tmp = stackalloc char[span1.Length + span2.Length + span3.Length];
            var span = new Span<char>(tmp, span1.Length + span2.Length + span3.Length);
            
            span1.CopyTo(span);
            span2.CopyTo(span);
            span3.CopyTo(span);

            return span.ToString();
        }
    }
}