using System.Collections.Generic;
using System.Text.RegularExpressions;
using CodeMetricsCalculator.Parsers.CodeInfo;
using CodeMetricsCalculator.Parsers.Pascal.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Pascal.Operators
{
    internal class AssignmentOperator : CommonOperator
    {
        private static readonly List<AssignmentOperator> AllOperators = new List<AssignmentOperator>
        {
            new AssignmentOperator("="),
            new AssignmentOperator("+="),
            new AssignmentOperator("-="),
            new AssignmentOperator("*="),
            new AssignmentOperator("/="),
            new AssignmentOperator("%="),
            new AssignmentOperator("&="),
            new AssignmentOperator("^="),
            new AssignmentOperator("|="),
            new AssignmentOperator("<<="),
            new AssignmentOperator(">>="),
            new AssignmentOperator(">>>=")
        };

        public AssignmentOperator(string operatorString)
            : base(operatorString, operatorString, OperationType.Binary, OperatorSyntax.Infix)
        {
        }

        public static IReadOnlyCollection<AssignmentOperator> All
        {
            get { return AllOperators.AsReadOnly(); }
        }
    }
}