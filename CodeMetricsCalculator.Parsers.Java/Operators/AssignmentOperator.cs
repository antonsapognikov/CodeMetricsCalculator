using System.Text.RegularExpressions;
using CodeMetricsCalculator.Parsers.CodeInfo;
using CodeMetricsCalculator.Parsers.Java.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java.Operators
{
    internal class AssignmentOperator : JavaOperator
    {
        private const string OperatorString = "=";

        public AssignmentOperator()
            : base(OperatorString, OperationType.Binary, OperatorSyntax.Infix)
        {
        }
    }
}