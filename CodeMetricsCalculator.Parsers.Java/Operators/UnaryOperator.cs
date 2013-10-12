using System.Text.RegularExpressions;

namespace CodeMetricsCalculator.Parsers.Java.Operators
{
    /// <summary>
    ///     Unary Java operator (+, -, ++, --, !).
    /// </summary>
    internal class UnaryOperator : JavaOperator
    {
        //todo: регулярка уныла
        public UnaryOperator(string operatorString)
            : base(operatorString, new Regex(string.Format(@"[^{0}]\{0}[^{0}=]", operatorString), RegexOptions.Compiled)
                )
        {
        }

        public UnaryOperator(string operatorString, Regex regex)
            : base(operatorString, regex)
        {
        }
    }
}