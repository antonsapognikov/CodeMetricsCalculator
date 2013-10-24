using System.Text.RegularExpressions;
using CodeMetricsCalculator.Parsers.CodeInfo;
using CodeMetricsCalculator.Parsers.Java.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java.Operators
{
    /// <summary>
    ///     Unary Java operator (+, -, ++, --, !).
    /// </summary>
    internal class UnaryOperator : JavaOperator
    {
        public UnaryOperator(string operatorString, OperatorSyntax operatorSyntax)
            : base(operatorString, OperationType.Unary, operatorSyntax)
        {
        }
    }
}