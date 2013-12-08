using System;
using System.Collections.Generic;
using CodeMetricsCalculator.Parsers.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Pascal.Operators
{
    /// <summary>
    ///     Conditional Pascal operator (&&, ||, ?:).
    /// </summary>
    internal class ConditionalOperator : CommonOperator
    {
        private static readonly List<ConditionalOperator> AllOperators = new List<ConditionalOperator>
        {
            new ConditionalOperator("and", "and", OperationType.Binary),
            new ConditionalOperator("and then", "and then", OperationType.Binary),
            new ConditionalOperator("or", "or", OperationType.Binary),
            new ConditionalOperator("or else", "or else", OperationType.Binary),
            new ConditionalOperator("not", "not", OperationType.Binary),
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