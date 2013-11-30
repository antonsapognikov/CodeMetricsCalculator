using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CodeMetricsCalculator.Parsers
{
    public class Pattern
    {
        public static readonly string Args = "{args}";
        public static readonly string Params = "{params}";
        public static readonly string Identifier = "{identifier}";
        public static readonly string Operand = "{operand}";

        private readonly string _pattern;
        private readonly bool _bracesRequires;

        public Pattern(string pattern) : this (pattern, false)
        {
        }

        public Pattern(string pattern, bool bracesRequires)
        {
            Contract.Requires(pattern != null, "pattern");

            _pattern = pattern;
            _bracesRequires = bracesRequires;
        }

        public bool BracesRequires
        {
            get { return _bracesRequires; }
        }

        /// <summary>
        /// Returns a string that represents Pattern.
        /// </summary>
        /// <returns>
        /// A string that represents Pattern.
        /// </returns>
        public override string ToString()
        {
            return _pattern;
        }
    }
}
