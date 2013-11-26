using System;
using System.Collections.Generic;
using CodeMetricsCalculator.Parsers.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java.Operators
{
    /// <summary>
    ///     Conditional Java operator (&&, ||, ?:).
    /// </summary>
    internal class ConditionalOperator : CommonOperator
    {
        private static readonly List<ConditionalOperator> AllOperators = new List<ConditionalOperator>
        {
            new ConditionalOperator("&&", "&&", OperationType.Binary),
            new ConditionalOperator("||", "||", OperationType.Binary),
            new ConditionalOperator("?:", null, OperationType.Ternary)
        };
        
        public ConditionalOperator(string operatorString, string keyword, OperationType operationType)
            : base(operatorString, keyword, operationType, OperatorSyntax.Infix)
        {
        }

        public static IReadOnlyCollection<ConditionalOperator> All
        {
            get { return AllOperators.AsReadOnly(); }
        }
    }
}