using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CodeMetricsCalculator.Parsers.CodeInfo;
using CodeMetricsCalculator.Parsers.Pascal.Operators;

namespace CodeMetricsCalculator.Parsers.Pascal.CodeInfo
{
    public abstract class PascalOperator : PascalCode, IOperatorInfo
    {
        static PascalOperator()
        {
            AllOperators = new List<PascalOperator>();
            AllOperators.AddRange(RelationalOperator.All);
            AllOperators.AddRange(AssignmentOperator.All);
            AllOperators.AddRange(ArithmeticOperator.All);
            AllOperators.AddRange(UnaryOperator.All);
            AllOperators.AddRange(BitwiseOperator.All);
            AllOperators.AddRange(ConditionalOperator.All);
            AllOperators.AddRange(PrimaryOperator.All);
            AllOperators.AddRange(BlockOperator.All);
        }
        
        private static readonly List<PascalOperator> AllOperators;

        private readonly Pattern _pattern;
        private readonly string _keyword;

        protected PascalOperator(Pattern pattern, string keyword)
            : this(pattern, pattern.ToString(), keyword)
        {
        }

        protected PascalOperator(Pattern pattern, string source, string keyword)
            : base(source)
        {
            _pattern = pattern;
            _keyword = keyword;
        }

        public static IReadOnlyCollection<PascalOperator> Operators
        {
            get { return AllOperators.AsReadOnly(); }
        }

        public string Name
        {
            get { return NormalizedSource; }
        }

        public Pattern Pattern
        {
            get { return _pattern; }
        }

        public bool IsKeywordBase
        {
            get { return _keyword != null; }
        }

        public string Keyword
        {
            get
            {
                if (!IsKeywordBase)
                    throw new InvalidOperationException("Operator is not based on keyword.");
                return _keyword;
            }
        }

        protected override string NormalizeSource(string originalSource)
        {
            return originalSource;
        }
    }
}