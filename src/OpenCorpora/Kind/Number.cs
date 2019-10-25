using System;
using System.Text.RegularExpressions;

namespace OpenCorpora.Kind
{
    public enum Number
    {
        // единственное число
        Sing = 1,

        // множественное число
        Plur = 2,
    }

    public static class NumberExtensions
    {
        private static readonly Regex TagEx = new Regex("(?x)(sing|plur)", RegexOptions.Compiled);

        public static bool TryParse(this string input, out Number kind)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("Value cannot be null or empty.", nameof(input));
            var match = TagEx.Match(input);

            switch (match.Captures[1].Value)
            {
                case "sing":
                {
                    kind = Number.Sing;
                    return true;
                }
                case "plur":
                {
                    kind = Number.Plur;
                    return true;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(input));
            }
        }

        public static string ToGrammeme(this Number kind)
        {
            switch(kind)
            {
                case Number.Sing: return "sign";
                case Number.Plur: return "plur";
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }
        }
        
        public static string Title(this Number kind)
        {
            switch (kind)
            {
                case Number.Sing: return "единственное число";
                case Number.Plur: return "множественное число";
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, "invalid kind value");
            }
        }
    }
}