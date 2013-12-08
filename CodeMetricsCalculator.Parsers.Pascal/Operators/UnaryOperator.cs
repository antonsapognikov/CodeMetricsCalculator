using System.Collections.Generic;
using System.Text.RegularExpressions;
using CodeMetricsCalculator.Parsers.CodeInfo;
using CodeMetricsCalculator.Parsers.Pascal.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Pascal.Operators
{
    /// <summary>
    ///     Unary Pascal operator (+, -, ++, --, !, ~).
    /// </summary>
    internal class UnaryOperator : CommonOperator
    {
        private static readonly List<UnaryOperator> AllOperators = new List<UnaryOperator>
        {
            new UnaryOperator("+", OperatorSyntax.Prefix),
            new UnaryOperator("-", OperatorSyntax.Prefix),
            new UnaryOperator("++", OperatorSyntax.Prefix),
            new UnaryOperator("--", OperatorSyntax.Prefix),
            new UnaryOperator("++", OperatorSyntax.Postfix),
            new UnaryOperator("--", OperatorSyntax.Postfix),
            new UnaryOperator("!", OperatorSyntax.Prefix),
            new UnaryOperator("~", OperatorSyntax.Prefix)
        };

        public UnaryOperator(string operatorString, OperatorSyntax syntax)
            : base(operatorString, operatorString, OperationType.Unary, syntax)
        {
        }

        public static IReadOnlyCollection<UnaryOperator> All
        {
            get { return AllOperators.AsReadOnly(); }
        }
    }
}