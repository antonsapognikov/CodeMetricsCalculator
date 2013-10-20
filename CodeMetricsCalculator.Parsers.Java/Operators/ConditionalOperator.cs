using System.Text.RegularExpressions;
using CodeMetricsCalculator.Parsers.CodeInfo;
using CodeMetricsCalculator.Parsers.Java.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java.Operators
{
    /// <summary>
    ///     Conditional Java operator (&&, ||, ?:).
    /// </summary>
    internal class ConditionalOperator : JavaOperator
    {
        public ConditionalOperator(string operatorString, OperationType operationType)
            : base(operatorString, operationType, OperatorSyntax.Infix)
        {
        }
    }
}