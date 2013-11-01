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
            AllOperators.AddRange(AssignmentOperator.All);
            AllOperators.AddRange(ArithmeticOperator.All);
            AllOperators.AddRange(UnaryOperator.All);
            AllOperators.AddRange(BitwiseOperator.All);
            AllOperators.AddRange(RelationalOperator.All);
            AllOperators.AddRange(ConditionalOperator.All);
            AllOperators.AddRange(PrimaryOperator.All);
            AllOperators.AddRange(BlockOperator.All);
        }
        
        private static readonly List<JavaOperator> AllOperators;

        private readonly Pattern _pattern;

        protected JavaOperator(Pattern pattern)
            : base(pattern.ToString())
        {
            _pattern = pattern;
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

        protected override string NormalizeSource(string originalSource)
        {
            return originalSource;
        }
    }
}