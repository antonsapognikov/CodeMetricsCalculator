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

        public Pattern(string pattern)
        {
            Contract.Requires(pattern != null, "pattern");

            _pattern = pattern;
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
