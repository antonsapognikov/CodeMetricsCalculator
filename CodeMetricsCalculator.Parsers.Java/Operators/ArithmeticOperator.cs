using System.Text.RegularExpressions;
using CodeMetricsCalculator.Parsers.CodeInfo;
using CodeMetricsCalculator.Parsers.Java.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java.Operators
{
    /// <summary>
    ///     Arithmetic Java operator (+, -, *, /, %).
    /// </summary>
    internal class ArithmeticOperator : JavaOperator
    {
        public ArithmeticOperator(string operatorString)
            : base(operatorString, OperationType.Binary, OperatorSyntax.Infix)
                
        {
        }
    }
}