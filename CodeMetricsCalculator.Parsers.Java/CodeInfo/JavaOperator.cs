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

        private readonly string _keyword;

        protected JavaOperator(string operatorString) : this(operatorString, null)
        {
        }

        protected JavaOperator(string operatorString, string keyword)
            : base(operatorString)
        {
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

        public bool IsKeywordBased
        {
            get { return _keyword != null; }
        }

        /// <summary>
        /// Operator keyword
        /// </summary>
        /// <exception cref="InvalidOperationException">Cannot get keyword for not keyword based operator.</exception>
        public string Keyword
        {
            get
            {
                if (!IsKeywordBased)
                    throw new InvalidOperationException("Cannot get keyword for not keyword based operator.");
                return _keyword;
            }
        }
    }
}