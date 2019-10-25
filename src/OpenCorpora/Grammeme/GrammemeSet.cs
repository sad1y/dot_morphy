using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace OpenCorpora.Grammeme
{
    public class GrammemeSet
    {
        public readonly HashSet<string> Data;

        private static readonly Regex Sep = new Regex("(,| )", RegexOptions.Compiled);
        
        public GrammemeSet(string raw)
        {
            if (string.IsNullOrEmpty(raw)) throw new ArgumentException("Value cannot be null or empty.", nameof(raw));
            Data = new HashSet<string>(Sep.Split(raw));
        }
    }
}