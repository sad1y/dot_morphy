using System;
using System.Text.RegularExpressions;

namespace OpenCorpora.Kind
{
    public enum Involvement
    {
        // говорящий включён в действие
        Incl = 1,

        // говорящий не включён в действие
        Excl = 2
    }

    public static class InvolvementExtensions
    {
        private static readonly Regex TagEx = new Regex("(?x)(incl|excl)", RegexOptions.Compiled);

        public static bool TryParse(this string input, out Involvement kind)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("Value cannot be null or empty.", nameof(input));
            var match = TagEx.Match(input);

            switch (match.Captures[1].Value)
            {
                case "incl":
                {
                    kind = Involvement.Incl;
                    return true;
                }
                case "excl":
                {
                    kind = Involvement.Excl;
                    return true;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(input));
            }
        }

        public static string Title(this Involvement kind)
        {
            switch (kind)
            {
                case Involvement.Incl: return "говорящий включён в действие";
                case Involvement.Excl: return "говорящий не включён в действие";
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, "invalid kind value");
            }
        }
    }
}