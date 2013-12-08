using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using CodeMetricsCalculator.Parsers.Pascal.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Pascal.Operators
{
    internal class BlockOperator : PascalOperator
    {
        public const string OpeningBlockBracket = "{";
        public const string ClosingBlockBracket = "}";
        
        private static readonly List<BlockOperator> AllOperators = new List<BlockOperator>
        {
            new BlockOperator("while (...) {...}", false, "while"),
            new BlockOperator("do {...} while (...)", false, "do"),
            new BlockOperator("for (...) {...}", false, "for"),
            new BlockOperator("if (...) {...}", false, "if"),
            new BlockOperator("else {...}", false, "else"),
            new BlockOperator("switch (...) {...}", true, "switch"),
            new BlockOperator("case " + Pattern.Identifier + " : {...}", false, "case"),
            new BlockOperator("default: {...}", false, "default"),
            new BlockOperator("try {...}", true, "try"),
            new BlockOperator("catch (" + Pattern.Identifier + " " + Pattern.Identifier + ") {...}", true, "catch"),
            new BlockOperator("finally {...}", true, "finally")
        };

        private readonly bool _bracketsRequired;

        public BlockOperator(string pattern, bool bracketsRequired, string keyword)
            : base(new Pattern(pattern, bracketsRequired), keyword)
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
