using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using CodeMetricsCalculator.Parsers.Java.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java.Operators
{
    internal class BlockOperator : JavaOperator
    {
        public const string OpeningBlockBracket = "{";
        public const string ClosingBlockBracket = "}";
        
        private static readonly List<BlockOperator> AllOperators = new List<BlockOperator>
        {
            new BlockOperator("while (...) {...}", false),
            new BlockOperator("do {...} while (...)", false),
            new BlockOperator("for (...;...;...) {...}", false),
            new BlockOperator("if (...) {...}", false),
            new BlockOperator("else {...}", false),
            new BlockOperator("switch (...) {...}", true),
            new BlockOperator("case " + Pattern.Identifier + " : {...}", false),
            new BlockOperator("default: {...}", false)
        };

        private readonly bool _bracketsRequired;

        public BlockOperator(string pattern, bool bracketsRequired)
            : base(new Pattern(pattern, bracketsRequired))
        {
            _bracketsRequired = bracketsRequired;
        }

        public static IReadOnlyCollection<BlockOperator> All
        {
            get { return AllOperators.AsReadOnly(); }
        }

        public bool BracketsRequired
        {
            get { return _bracketsRequired; }
        }
    }
}
