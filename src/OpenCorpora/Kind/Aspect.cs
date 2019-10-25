using System;
using System.Text.RegularExpressions;

namespace OpenCorpora.Kind
{
    public enum Aspect
    {
        /// совершенный вид
        Perf = 1,
        /// несовершенный вид
        Impf = 2
    }
    
    public static class AspectExtensions
    {
        private static readonly Regex TagEx = new Regex("(?x)(perf|impf)", RegexOptions.Compiled);

        public static bool TryParse(this string input, out Aspect kind)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("Value cannot be null or empty.", nameof(input));
            var match = TagEx.Match(input);

            switch (match.Captures[1].Value)
            {
                case "perf":
                {
                    kind = Aspect.Perf;
                    return true;
                }
                case "impf":
                {
                    kind = Aspect.Impf;
                    return true;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(input));
            }
        }

        public static string Title(this Aspect kind)
        {
            switch (kind)
            {
                case Aspect.Perf:
                    return "совершенный вид";
                case Aspect.Impf:
                    return "несовершенный вид";
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, "invalid kind value");
            }
        }
    }
}