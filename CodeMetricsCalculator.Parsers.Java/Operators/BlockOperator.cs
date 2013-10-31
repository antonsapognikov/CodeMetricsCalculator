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

        static BlockOperator()
        {
            AllOperators = new List<BlockOperator>
            {
                While,
                Do,
                For,
                If,
                Else,
                Switch,
                Case,
                Default
            };
        }

        private static readonly List<BlockOperator> AllOperators;
        private readonly bool _bracketsRequired;

        public BlockOperator(string operatorString, string keyword, bool bracketsRequired)
            : base(operatorString, keyword)
        {
            Contract.Requires<ArgumentNullException>(keyword != null, "keyword");

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

        public static readonly BlockOperator While = new BlockOperator("while (...) {...}", "while", false);
        public static readonly BlockOperator Do = new BlockOperator("do {...}", "do", false);
        public static readonly BlockOperator For = new BlockOperator("for (...;...;...) {...}", "for", false);
        public static readonly BlockOperator If = new BlockOperator("if (...) {...}", "if", false);
        public static readonly BlockOperator Else = new BlockOperator("else {...}", "else", false);
        public static readonly BlockOperator Switch = new BlockOperator("switch (...) {...}", "switch", true);
        public static readonly BlockOperator Case = new BlockOperator("case x: {...}", "case", false);
        public static readonly BlockOperator Default = new BlockOperator("default: {...}", "default", false);
    }
}
