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
            new ConditionalOperator("&&", OperationType.Binary),
            new ConditionalOperator("||", OperationType.Binary),
            new ConditionalOperator("?:", OperationType.Ternary)
        };
        
        public ConditionalOperator(string operatorString, OperationType operationType)
            : base(operatorString, operationType, OperatorSyntax.Infix)
        {
        }

        public static IReadOnlyCollection<ConditionalOperator> All
        {
            get { return AllOperators.AsReadOnly(); }
        }
    }
}