using System;
using System.Text.RegularExpressions;

namespace OpenCorpora.Kind
{
    public enum Mood
    {
        // изъявительное наклонение
        Indc = 1,

        // повелительное наклонение
        Impr = 2,
    }

    public static class MoodExtensions
    {
        private static readonly Regex TagEx = new Regex("(?x)(indc|impr)", RegexOptions.Compiled);

        public static bool TryParse(this string input, out Mood kind)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("Value cannot be null or empty.", nameof(input));
            var match = TagEx.Match(input);

            switch (match.Captures[1].Value)
            {
                case "indc":
                {
                    kind = Mood.Indc;
                    return true;
                }
                case "impr":
                {
                    kind = Mood.Impr;
                    return true;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(input));
            }
        }

        public static string Title(this Mood kind)
        {
            switch (kind)
            {
                case Mood.Indc: return "изъявительное наклонение";
                case Mood.Impr: return "повелительное наклонение";
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, "invalid kind value");
            }
        }
    }
}