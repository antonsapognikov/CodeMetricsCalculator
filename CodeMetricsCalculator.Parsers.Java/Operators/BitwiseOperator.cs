using System.Text.RegularExpressions;
using CodeMetricsCalculator.Parsers.CodeInfo;
using CodeMetricsCalculator.Parsers.Java.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java.Operators
{
    /// <summary>
    ///     Bitwise and Bit Shift Operators (~, <<, >>, >>>, &, |, ^).
    /// </summary>
    internal class BitwiseOperator : JavaOperator
    {
        public BitwiseOperator(string operatorString)
            : base(operatorString, OperationType.Binary, OperatorSyntax.Infix)
        {
        }
    }
}