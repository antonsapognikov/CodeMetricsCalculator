using System.Text.RegularExpressions;

namespace CodeMetricsCalculator.Parsers.Java.Operators
{
    /// <summary>
    ///     Conditional Java operator (&&, ||, ?:).
    /// </summary>
    internal class ConditionalOperator : JavaOperator
    {
        //todo: как-то ограничить, чтобы и слева, и справа от операторы были операнды
        public ConditionalOperator(string operatorString)
            : base(operatorString, new Regex(string.Format(@"[^{0}]{0}[^{0}=]", operatorString), RegexOptions.Compiled))
        {
        }

        public ConditionalOperator(string operatorString, Regex regex)
            : base(operatorString, regex)
        {
        }
    }
}