using System;
using System.Text.RegularExpressions;

namespace OpenCorpora.Kind
{
    public enum Gender
    {
        // мужской род
        Masc = 1,

        // женский род
        Femn = 2,

        // средний род
        Neut = 3,
    }

    public static class GenderExtensions
    {
        private static readonly Regex TagEx = new Regex("(?x)(masc|femn|neut)", RegexOptions.Compiled);

        public static bool TryParse(this string input, out Gender kind)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("Value cannot be null or empty.", nameof(input));

            var match = TagEx.Match(input);

            switch (match.Captures[1].Value)
            {
                case "masc":
                {
                    kind = Gender.Masc;
                    return true;
                }
                case "femn":
                {
                    kind = Gender.Femn;
                    return true;
                }
                case "neut":
                {
                    kind = Gender.Neut;
                    return true;
                }
                default:
                {
                    kind = 0;
                    return false;
                }
            }
        }

        public static string Title(this Gender kind)
        {
            switch (kind)
            {
                case Gender.Masc: return "мужской род";
                case Gender.Femn: return "женский род";
                case Gender.Neut: return "средний род";
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, "invalid kind value");
            }
        }
    }
}