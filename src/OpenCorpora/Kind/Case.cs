using System;
using System.Text.RegularExpressions;

namespace OpenCorpora.Kind
{
    public enum Case
    {
        // именительный падеж
        Nomn = 1,

        // родительный падеж
        Gent = 2,

        // дательный падеж
        Datv = 3,

        // винительный падеж
        Accs = 4,

        // творительный падеж
        Ablt = 5,

        // предложный падеж
        Loct = 6,

        // звательный падеж
        Voct = 7,

        // первый родительный падеж
        Gen1 = 8,

        // второй родительный (частичный) падеж
        Gen2 = 9,

        // второй винительный падеж
        Acc2 = 10,

        // первый предложный падеж
        Loc1 = 11,

        // второй предложный (местный) падеж
        Loc2 = 12,
    }

    public static class CaseExtensions
    {
        private static readonly Regex TagEx =
            new Regex("(?x)(nomn|gent|datv|accs|ablt|loct|voct|gen1|gen2|acc2|loc1|loc2)", RegexOptions.Compiled);

        public static bool TryParse(this string input, out Case kind)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("Value cannot be null or empty.", nameof(input));
            var match = TagEx.Match(input);

            switch (match.Captures[1].Value)
            {
                case "nomn":
                {
                    kind = Case.Nomn;
                    return true;
                }
                case "gent":
                {
                    kind = Case.Gent;
                    return true;
                }
                case "datv":
                {
                    kind = Case.Datv;
                    return true;
                }
                case "accs":
                {
                    kind = Case.Accs;
                    return true;
                }
                case "ablt":
                {
                    kind = Case.Ablt;
                    return true;
                }
                case "loct":
                {
                    kind = Case.Loct;
                    return true;
                }
                case "voct":
                {
                    kind = Case.Voct;
                    return true;
                }
                case "gen1":
                {
                    kind = Case.Gen1;
                    return true;
                }
                case "gen2":
                {
                    kind = Case.Gen2;
                    return true;
                }
                case "acc2":
                {
                    kind = Case.Acc2;
                    return true;
                }
                case "loc1":
                {
                    kind = Case.Loc1;
                    return true;
                }
                case "loc2":
                {
                    kind = Case.Loc2;
                    return true;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(input));
            }
        }

        public static string ToGrammeme(this Case kind)
        {
            switch (kind)
            {
                case Case.Nomn: return "nomn";
                case Case.Gent: return "gent";
                case Case.Datv: return "datv";
                case Case.Accs: return "accs";
                case Case.Ablt: return "ablt";
                case Case.Loct: return "loct";
                case Case.Voct: return "voct";
                case Case.Gen1: return "gen1";
                case Case.Gen2: return "gen2";
                case Case.Acc2: return "acc2";
                case Case.Loc1: return "loc1";
                case Case.Loc2: return "loc2";
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind));
            }
        }

        public static string Title(this Case kind)
        {
            switch (kind)
            {
                case Case.Nomn: return "именительный падеж";
                case Case.Gent: return "родительный падеж";
                case Case.Datv: return "дательный падеж";
                case Case.Accs: return "винительный падеж";
                case Case.Ablt: return "творительный падеж";
                case Case.Loct: return "предложный падеж";
                case Case.Voct: return "звательный падеж";
                case Case.Gen1: return "первый родительный падеж";
                case Case.Gen2: return "второй родительный (частичный) падеж";
                case Case.Acc2: return "второй винительный падеж";
                case Case.Loc1: return "первый предложный падеж";
                case Case.Loc2: return "второй предложный (местный) падеж";
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, "invalid kind value");
            }
        }
    }
}