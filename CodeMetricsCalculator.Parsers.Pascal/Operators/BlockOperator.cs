using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using CodeMetricsCalculator.Parsers.Pascal.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Pascal.Operators
{
    internal class BlockOperator : PascalOperator
    {
        public const string OpeningBlockBracket = "begin";
        public const string ClosingBlockBracket = "end;";
        
        private static readonly List<BlockOperator> AllOperators = new List<BlockOperator>
        {
            new BlockOperator("repeat"),
            new BlockOperator("until"),
            new BlockOperator("while"),
            new BlockOperator("do"),
            new BlockOperator("for"),
            new BlockOperator("to"),
            new BlockOperator("downto"),
            new BlockOperator("if"),
            new BlockOperator("then"),
            new BlockOperator("else"),
            new BlockOperator("case"),
            new BlockOperator("of"),
            new BlockOperator("on"),
            new BlockOperator("catch"),
            new BlockOperator("finally"),
            new BlockOperator("with")
        };


        public BlockOperator(string keyword)
            : base(new Pattern(keyword), keyword)
        {
        }

        public static IReadOnlyCollection<BlockOperator> All
        {
            get { return AllOperators.AsReadOnly(); }
        }
    }
}
