using System.Text.RegularExpressions;
using CodeMetricsCalculator.Parsers.CodeInfo;
using CodeMetricsCalculator.Parsers.Java.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java.Operators
{
    internal class InstanceofOperator : JavaOperator
    {
        public InstanceofOperator()
            : base("instanceof", OperationType.Unary, OperatorSyntax.Prefix)
        {
        }
    }
}