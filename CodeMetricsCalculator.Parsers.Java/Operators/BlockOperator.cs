﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.Java.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java.Operators
{
    internal class BlockOperator : JavaOperator
    {
        static BlockOperator()
        {
            AllOperators = new List<BlockOperator>
            {
                While,
                Do,
                For,
                If,
                IfElse,
                Switch
            };
        }

        private static readonly List<BlockOperator> AllOperators;
        
        public BlockOperator(string operatorString) : base(operatorString, true, false)
        {
        }

        public static IReadOnlyCollection<BlockOperator> All
        {
            get { return AllOperators.AsReadOnly(); }
        }

        public static readonly BlockOperator While = new BlockOperator("while (...) {...}");
        public static readonly BlockOperator Do = new BlockOperator("do {...}");
        public static readonly BlockOperator For = new BlockOperator("for (...;...;...) {...}");
        public static readonly BlockOperator If = new BlockOperator("if (...) {...}");
        public static readonly BlockOperator Else = new BlockOperator("else {...}");
        public static readonly BlockOperator Switch = new BlockOperator("switch (...) {...}");
    }
}
