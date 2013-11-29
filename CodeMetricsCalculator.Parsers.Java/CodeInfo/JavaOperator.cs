using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CodeMetricsCalculator.Parsers.CodeInfo;
using CodeMetricsCalculator.Parsers.Java.Operators;

namespace CodeMetricsCalculator.Parsers.Java.CodeInfo
{
    public abstract class JavaOperator : JavaCode, IOperatorInfo
    {
        static JavaOperator()
        {
            AllOperators = new List<JavaOperator>();
            AllOperators.AddRange(RelationalOperator.All);
            AllOperators.AddRange(AssignmentOperator.All);
            AllOperators.AddRange(ArithmeticOperator.All);
            AllOperators.AddRange(UnaryOperator.All);
            AllOperators.AddRange(BitwiseOperator.All);
            AllOperators.AddRange(ConditionalOperator.All);
            AllOperators.AddRange(PrimaryOperator.All);
            AllOperators.AddRange(BlockOperator.All);
        }
        
        private static readonly List<JavaOperator> AllOperators;

        private readonly Pattern _pattern;
        private readonly string _keyword;

        protected JavaOperator(Pattern pattern, string keyword)
            : this(pattern, pattern.ToString(), keyword)
        {
        }

        protected JavaOperator(Pattern pattern, string source, string keyword)
            : base(source)
        {
            _pattern = pattern;
            _keyword = keyword;
        }

        public static IReadOnlyCollection<JavaOperator> Operators
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