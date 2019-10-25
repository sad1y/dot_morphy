using System;
using System.Text.RegularExpressions;

namespace OpenCorpora.Kind
{
    public enum Person
    {
        // 1 лицо
        Per1 = 1,

        // 2 лицо
        Per2 = 2,

        // 3 лицо
        Per3 = 3,
    }

    public static class PersonExtensions
    {
        private static readonly Regex TagEx = new Regex("(?x)(1per|2per|3per)", RegexOptions.Compiled);

        public static bool TryParse(this string input, out Person kind)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("Value cannot be null or empty.", nameof(input));
            var match = TagEx.Match(input);

            switch (match.Captures[1].Value)
            {
                case "1per":
                {
                    kind = Person.Per1;
                    return true;
                }
                case "2per":
                {
                    kind = Person.Per2;
                    return true;
                }
                case "3per":
                {
                    kind = Person.Per3;
                    return true;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(input));
            }
        }

        public static string Title(this Person kind)
        {
            switch (kind)
            {
                case Person.Per1: return "1 лицо";
                case Person.Per2: return "2 лицо";
                case Person.Per3: return "3 лицо";
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, "invalid kind value");
            }
        }
    }
}