using System;
using System.Text.RegularExpressions;

namespace OpenCorpora.Kind
{
    public enum Animacy
    {
        // одушевлённое
        Anim = 1,

        // неодушевлённое
        Inan = 2
    }

    public static class AnimacyExtensions
    {
        private static readonly Regex TagEx = new Regex("(?x)(anim|inan)", RegexOptions.Compiled);

        public static bool TryParse(this string input, out Animacy kind)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("Value cannot be null or empty.", nameof(input));
            var match = TagEx.Match(input);

            switch (match.Captures[1].Value)
            {
                case "anim":
                {
                    kind = Animacy.Anim;
                    return true;
                }
                case "inan":
                {
                    kind = Animacy.Inan;
                    return true;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(input));
            }
        }

        public static string Title(this Animacy kind)
        {
            switch (kind)
            {
                case Animacy.Anim:
                    return "одушевлённое";
                case Animacy.Inan:
                    return "неодушевлённое";
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, "invalid kind value");
            }
        }
    }
}