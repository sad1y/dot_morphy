using System;
using System.Text.RegularExpressions;

namespace OpenCorpora.Kind
{
    public enum Pos
    {
        // имя существительное
        Noun = 1,

        // имя прилагательное (полное)
        Adjf,

        // имя прилагательное (краткое)
        Adjs,

        // компаратив
        Comp,

        // глагол (личная форма)
        Verb,

        // глагол (инфинитив)
        Infn,

        // причастие (полное)
        Prtf,

        // причастие (краткое)
        Prts,

        // деепричастие
        Grnd,

        // числительное
        Numr,

        // наречие
        Advb,

        // местоимение-существительное
        Npro,

        // предикатив
        Pred,

        // предлог
        Prep,

        // союз
        Conj,

        // частица
        Prcl,

        // междометие
        Intj,
    }

    public static class PosExtensions
    {
        private static readonly Regex TagEx =
            new Regex("(?x)(NOUN|ADJF|ADJS|COMP|VERB|INFN|PRTF|PRTS|GRND|NUMR|ADVB|NPRO|PRED|PREP|CONJ|PRCL|INTJ)",
                RegexOptions.Compiled);

        public static bool TryParse(this string input, out Pos kind)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("Value cannot be null or empty.", nameof(input));
            var match = TagEx.Match(input);

            switch (match.Captures[1].Value)
            {
                case "NOUN":
                {
                    kind = Pos.Noun;
                    return true;
                }
                case "ADJF":
                {
                    kind = Pos.Adjf;
                    return true;
                }
                case "ADJS":
                {
                    kind = Pos.Adjs;
                    return true;
                }
                case "COMP":
                {
                    kind = Pos.Comp;
                    return true;
                }
                case "VERB":
                {
                    kind = Pos.Verb;
                    return true;
                }
                case "INFN":
                {
                    kind = Pos.Infn;
                    return true;
                }
                case "PRTF":
                {
                    kind = Pos.Prtf;
                    return true;
                }
                case "PRTS":
                {
                    kind = Pos.Prts;
                    return true;
                }
                case "GRND":
                {
                    kind = Pos.Grnd;
                    return true;
                }
                case "NUMR":
                {
                    kind = Pos.Numr;
                    return true;
                }
                case "ADVB":
                {
                    kind = Pos.Advb;
                    return true;
                }
                case "NPRO":
                {
                    kind = Pos.Npro;
                    return true;
                }
                case "PRED":
                {
                    kind = Pos.Pred;
                    return true;
                }
                case "PREP":
                {
                    kind = Pos.Prep;
                    return true;
                }
                case "CONJ":
                {
                    kind = Pos.Conj;
                    return true;
                }
                case "PRCL":
                {
                    kind = Pos.Prcl;
                    return true;
                }
                case "INTJ":
                {
                    kind = Pos.Intj;
                    return true;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(input));
            }
        }

        public static string Title(this Pos kind)
        {
            switch (kind)
            {
                case Pos.Noun: return "имя существительное";
                case Pos.Adjf: return "имя прилагательное (полное)";
                case Pos.Adjs: return "имя прилагательное (краткое)";
                case Pos.Comp: return "компаратив";
                case Pos.Verb: return "глагол (личная форма)";
                case Pos.Infn: return "глагол (инфинитив)";
                case Pos.Prtf: return "причастие (полное)";
                case Pos.Prts: return "причастие (краткое)";
                case Pos.Grnd: return "деепричастие";
                case Pos.Numr: return "числительное";
                case Pos.Advb: return "наречие";
                case Pos.Npro: return "местоимение-существительное";
                case Pos.Pred: return "предикатив";
                case Pos.Prep: return "предлог";
                case Pos.Conj: return "союз";
                case Pos.Prcl: return "частица";
                case Pos.Intj: return "междометие";
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, "invalid kind value");
            }
        }
    }
}