using System.Collections.Generic;
using System.Text.RegularExpressions;
using CodeMetricsCalculator.Parsers.CodeInfo;
using CodeMetricsCalculator.Parsers.Java.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java.Operators
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
            : base(operatorString, OperationType.Binary, OperatorSyntax.Infix)
        {
        }

        public static IReadOnlyCollection<AssignmentOperator> All
        {
            get { return AllOperators.AsReadOnly(); }
        }
    }
}