using System.Text.RegularExpressions;

namespace CodeMetricsCalculator.Parsers.Java.Operators
{
    internal class AssignmentOperator : JavaOperator
    {
        private const string OperatorString = "=";

        public AssignmentOperator()
            : base(OperatorString, new Regex(string.Format(@"[^{0}]{0}[^{0}]", OperatorString), RegexOptions.Compiled))
        {
            //todo: как-то ограничить, чтобы и слева, и справа от операторы были операнды
        }
    }
}